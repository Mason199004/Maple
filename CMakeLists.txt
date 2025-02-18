cmake_minimum_required(VERSION 3.27)
project(Maple C)

set(CMAKE_C_STANDARD 23)

add_executable(Maple main.c
        Project/MapleProj.c
        plugins/plugins.c
        MapleTypes.c
		logging/logging.c
		core/core.c
		plugins/core_config_provider.c
)
