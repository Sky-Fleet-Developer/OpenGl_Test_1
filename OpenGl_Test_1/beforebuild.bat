IF EXIST %1Resources (
	rd /s/q %1Resources
    echo ����� %1Resources �������.
) else (
    echo ����� %1Resources �� ����������.
)