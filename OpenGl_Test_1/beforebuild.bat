IF EXIST %1Resources (
	rd /s/q %1Resources
    echo Папка %1Resources удалена.
) else (
    echo Папка %1Resources не существует.
)