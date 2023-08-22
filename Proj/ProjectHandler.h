#ifndef MAPLE_PROJECTHANDLER_H
#define MAPLE_PROJECTHANDLER_H

#include "Project.h"

i32 SaveLocalProject(const char* path);

i32 LoadLocalProject(const char* path);

i32 SaveGlobalProject();

i32 LoadGlobalProject();

i32 SaveHiddenProject(const char* path);

i32 LoadHiddenProject(const char* path);

ProjNode* GetNodeFromProj(Maple_Project* proj, const char* name, i32 NodeNum);

ProjNode* GetNode(const char* name, i32 NodeNum);

i32 SetupEnv(const char* ProjDir);

#endif //MAPLE_PROJECTHANDLER_H
