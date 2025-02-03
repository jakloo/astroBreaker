public enum SpawnCommand{
    SPAWN_DENIED,
    SPAWN_REGULAR,
    SPAWN_UPGRADE
}

public enum UpgradeType{
    LIFE,
    FREEZE,
    DOUBLE,
    BOOM
}

public enum GameMode {
    SPACE,
    ATMOSPHERE
}

public enum GuiScreen {
    GAME,
    PAUSE,
    END_GAME
}

public enum MenuGuiScreen {
    MAIN,
    GAME_MODE,
    COLORS
}


public enum ButtonType{
    RESTART,
    UNPAUSE,
    END_GAME,
    LEAVE_TO_MENU,
    QUIT_APP,
    NEW_GAME,
    CHOOSE_SPACE,
    CHOOSE_ATMOSPHERE,
    BACK_TO_MAIN,
    CHOOSE_NEON,
    CHOOSE_PASTEL,
    BACK_TO_GAME_MODE
}