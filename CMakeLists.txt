cmake_minimum_required(VERSION 3.27)
project(Maple C)

set(CMAKE_C_STANDARD 23)

add_executable(Maple main.c
        Project/MapleProj.c
        Project/MapleProj.h
        plugins/plugins.c
        plugins/plugins.h
        MapleTypes.c)
