#ifndef MAPLE_PROJECT_H
#define MAPLE_PROJECT_H
#include "../base.h"
#include "../Arena/Arena.h"
#include <stdio.h>

typedef struct {
	char Name[16];
	void* ValueOrPointer;
} ProjNode;

typedef struct {
	char MAGIC[5];
	u64 reserved;
	Arena arena;
	u64 NodeCount;
	ProjNode nodes[];
} Maple_Project;

extern Maple_Project* GlobalProject;
extern Maple_Project* LocalProject;

i32 GenerateNewProjectFromDefaults(const char* name);

i32 SaveProject(const char* path, Maple_Project* proj);

i32 LoadProject(const char* path, Maple_Project* proj);

#endif //MAPLE_PROJECT_H
