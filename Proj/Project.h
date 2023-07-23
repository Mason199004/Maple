#ifndef MAPLE_PROJECT_H
#define MAPLE_PROJECT_H
#include "../base.h"
#include <stdio.h>

i32 SaveLocalProject(const char* path);

i32 LoadLocalProject(const char* path);

i32 GenerateNewProjectFromDefaults(const char* name);

#endif //MAPLE_PROJECT_H
