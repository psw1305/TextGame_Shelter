﻿using ConsoleTables;
using Shelter.Core;
using Shelter.Model.Item;
using static System.Console;

namespace Shelter.Screen;

public class ScreenInventory : IScreen
{
    private static List<IItem> screenInventory = Game.player.Inventory.ToList();
    private static int currentIdx = 0;
    private static string[] selectNames =
    {
        "장비관리",
        "나가기",
    };

    private static ScreenType SelectScreen()
    {
        return currentIdx switch
        {
            0 => ScreenType.Equipment,
            _ => ScreenType.Main,
        };
    }

    /// <summary>
    /// 인벤토리 아이템 리스트 전시
    /// </summary>
    private void DrawInventoryList()
    {
        var table = new ConsoleTable("번 호", "이 름", "타 입", "설 명", "가 격");

        for (int i = 0; i < screenInventory.Count; i++)
        {
            table.AddRow(i + 1, screenInventory[i].Name, screenInventory[i].ItemType.TypeToString(), screenInventory[i].Desc, screenInventory[i].Price);
        }

        table.Write();
        WriteLine();
    }

    // 인벤토리 정보 표시
    public void DrawScreen()
    {
        do
        {
            Clear();
            WriteLine();
            WriteLine("[ 인 벤 토 리 ]");
            WriteLine();

            // 모든 아이템 목록 전시
            DrawInventoryList();

            for (int i = 0; i < selectNames.Length; i++)
            {
                if (i == currentIdx)
                {
                    WriteLine($"▷ {selectNames[i]}");
                }
                else
                {
                    WriteLine($"   {selectNames[i]}");
                }
            }

            WriteLine();
            WriteLine("[방향키 ↑ ↓: 위 아래로 이동] [Enter: 선택] [Esc: 메인으로 돌아가기]");
            WriteLine("[정렬 : 1. 이름  2. 가격  3. 타입]");
        }
        while (ManageInput());
    }

    public bool ManageInput()
    {
        var key = ReadKey(true);

        var commands = key.Key switch
        {
            ConsoleKey.UpArrow => Command.MoveTop,
            ConsoleKey.DownArrow => Command.MoveBottom,
            ConsoleKey.Enter => Command.Interact,
            ConsoleKey.Escape => Command.Exit,
            ConsoleKey.D1 => Command.Num1,
            ConsoleKey.D2 => Command.Num2,
            ConsoleKey.D3 => Command.Num3,
            _ => Command.Nothing
        };

        OnCommand(commands);
        return commands != Command.Exit;
    }

    static void OnCommand(Command cmd)
    {
        switch (cmd)
        {
            case Command.MoveTop:
                if (currentIdx > 0)
                {
                    currentIdx--;
                }
                break;
            case Command.MoveBottom:
                if (currentIdx < selectNames.Length - 1)
                {
                    currentIdx++;
                }
                break;
            case Command.Interact:
                Game.screen.DisplayScreen(SelectScreen());
                break;
            case Command.Exit:
                Game.screen.DisplayScreen(ScreenType.Main);
                break;
            case Command.Num1:
                screenInventory = Game.player.Inventory.OrderBy(item => item.Name).ToList();
                break;
            case Command.Num2:
                screenInventory = Game.player.Inventory.OrderByDescending(item => item.Price).ToList();
                break;
            case Command.Num3:
                screenInventory = Game.player.Inventory.OrderBy(item => item.ItemType).ToList();
                break;
            default:
                break;
        }
    }
}
