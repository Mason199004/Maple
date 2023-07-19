#ifndef MAPLE_PROJECT_H
#define MAPLE_PROJECT_H
#include "../base.h"
#include <stdio.h>

/* Handled Nodes  |
 * AUTO_ADD_SRC   | bool
 * RECURSE_DIR    | bool
 * DEF_LANG       | DefineLang
 * DEF_SRC        | DefineSrc
 * DEF_COMPILER   | DefineCompiler
 * */

#define AUTOSRC "AUTO_ADD_SRC"
#define RECURSE "RECURSE_DIR"
#define DEFLANG "DEF_LANG"
#define DEFSRC "DEF_SRC"
#define DEFCOMP "DEF_COMPILER"

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
    char Extensions[8][4]; //valid extensions for this language
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

typedef enum {
    OptimizeSpeed,
    OptimizeSize,
    Output,
} CompArgs;

typedef struct {
    CompArgs MapleArg;
    u32 ArgLen;
    char Arg[];
} CompArgKV;

typedef struct {
    char Name[16]; //look in path or call /usr/bin/env
    char Lang[8];
    u32 ArgCount;
    CompArgKV ArgMap[];
} DefineCompiler;

#endif //MAPLE_PROJECT_H
