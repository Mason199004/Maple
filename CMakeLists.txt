cmake_minimum_required(VERSION 3.25)
project(Maple C)

set(CMAKE_C_STANDARD 99)

add_executable(Maple main.c base.h Proj/Project.c Proj/Project.h Arena/Arena.c Arena/Arena.h IO/IO.c IO/IO.h StrUtils/StrUtils.c StrUtils/StrUtils.h
        Proj/ProjectHandler.c
        Proj/ProjectHandler.h)
