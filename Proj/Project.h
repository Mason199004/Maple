#ifndef MAPLE_PROJECT_H
#define MAPLE_PROJECT_H
#include "../base.h"
#include <stdio.h>

/* Handled Nodes  |
 * AUTO_ADD_SRC   | bool
 * RECURSE_DIR    | bool
 * DEF_LANG       | DefineLang
 * DEF_SRC        | DefineSrc
 * */

#define AUTOSRC "AUTO_ADD_SRC"
#define RECURSE "RECURSE_DIR"
#define DEFLANG "DEF_LANG"
#define DEFSRC "DEF_SRC"

typedef struct {
	char Name[16];
	void* ValueOrPointer;
} ProjNode;

typedef struct {
	char MAGIC[5];
	u64 reserved;
	u64 NodeCount;
	ProjNode nodes[];
} Maple_Project;

typedef struct {
	char Name[8];
	char Compiler[32]; //pass to /usr/bin/env or find in PATH
	u32 MetaLen;
	char Metadata[]; //currently unused
} DefineLang;

typedef struct {
	char Lang[8];
	//u64 ArenaSize;
	//arena* PathData;
	//u64 FileCount;
	//char* SrcFiles[]
	//TODO later, arena the strings for src paths, list of pointers into arena
} DefineSrc;

#endif //MAPLE_PROJECT_H
