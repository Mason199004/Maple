cmake_minimum_required(VERSION 3.25)
project(Maple C)

set(CMAKE_C_STANDARD 90)

add_executable(Maple main.c base.h Proj/Project.c Proj/Project.h)
