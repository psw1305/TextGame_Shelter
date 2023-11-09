﻿using Shelter.Core;

namespace Shelter.Screen;

public class ScreenMain : IScreen
{
    public static int currentIdx = 0;
    public static string[] selectNames =
    {
        "상 태 보 기",
        "인 벤 토 리",
        "쉘 터 찾 기"
    };

    private static ScreenType SelectScreen()
    {
        return currentIdx switch
        {
            0 => ScreenType.MyInfo,
            1 => ScreenType.Inventory,
            2 => ScreenType.Stage,
            _ => ScreenType.Main,
        };
    }

    public void DrawScreen()
    {
        do
        {
            Console.Clear();
            Console.WriteLine(Globals.ASCIIART_TITLE);
            Console.WriteLine();

            for (int i = 0; i < selectNames.Length; i++)
            {
                if (i == currentIdx)
                {
                    Console.WriteLine($"▷ {selectNames[i]}");
                }
                else
                {
                    Console.WriteLine($"   {selectNames[i]}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("[방향키 ↑ ↓: 위 아래로 이동] [Enter: 선택]");
        }
        while (ManageInput());
    }

    public bool ManageInput()
    {
        var key = Console.ReadKey(true);

        var commands = key.Key switch
        {
            ConsoleKey.UpArrow => Command.MoveTop,
            ConsoleKey.DownArrow => Command.MoveBottom,
            ConsoleKey.Enter => Command.Interact,
            _ => Command.Nothing
        };

        OnCommand(commands);
        return commands != Command.Interact;
    }

    static void OnCommand(Command cmd)
    {
        switch (cmd)
        {
            case Command.MoveTop:
                if (currentIdx > 0)
                    currentIdx--;
                break;

            case Command.MoveBottom:
                if (currentIdx < selectNames.Length - 1)
                    currentIdx++;
                break;

            case Command.Interact:
                Game.screen.DisplayScreen(SelectScreen());
                break;

            default:
                break;
        }
    }
}