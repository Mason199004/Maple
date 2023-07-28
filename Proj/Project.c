#include <string.h>
#include <malloc.h>
#include "Project.h"


/* Handled Nodes  |
 * AUTO_ADD_SRC   | bool
 * RECURSE_DIR    | bool
 * DEF_LANG       | DefineLang
 * DEF_SRC        | DefineSrc
 * DEF_COMPILER   | DefineCompiler
 * NAME           | char* arena'd
 * */

#define AUTOSRC "AUTO_ADD_SRC"
#define RECURSE "RECURSE_DIR"
#define DEFLANG "DEF_LANG"
#define DEFSRC "DEF_SRC"
#define DEFCOMP "DEF_COMPILER"
#define NAME "NAME"



typedef struct {
	char Name[8];
	char Extensions[8][4]; //valid extensions for this language
	u32 MetaLen;
	char Metadata[]; //currently unused
} DefineLang;

typedef struct {
	char Lang[8];
	u64 FileCount;
	char* SrcFiles[]; //in arena
} DefineSrc;

typedef enum {
	OptimizeSpeed,
	OptimizeSize,
	Output,
} CompArgs;

typedef struct {
	CompArgs MapleArg;
	u32 ArgLen;
	char* Arg; //in arena
} CompArgKV;

typedef struct {
	char Name[16]; //look in path or call /usr/bin/env
	char Lang[8];
	u32 ArgCount;
	CompArgKV ArgMap[];
} DefineCompiler;

Maple_Project* GlobalProject;
Maple_Project* LocalProject;

#define StrEq(X) strcmp(node->Name, X)

u64 WriteNode(ProjNode* node, FILE* file, u64 DataLoc, ArenaPtrMap* map) //yes the error handling here leaks some memory, cry about it
{
	if (StrEq(AUTOSRC) == 0)
	{
		return fwrite(node, sizeof(ProjNode), 1, file);
	}
	else if (StrEq(RECURSE) == 0)
	{
		return fwrite(node, sizeof(ProjNode), 1, file);
	}
	else if (StrEq(DEFLANG) == 0)
	{
		DefineLang* def = node->ValueOrPointer;
		node->ValueOrPointer = (void *) DataLoc;

		if (fwrite(node, sizeof(ProjNode), 1, file) == 0) return 0;

		fseek(file, DataLoc, SEEK_SET);

		if (fwrite(def, sizeof(DefineLang) + def->MetaLen, 1, file) == 0) return 0;
		u64 temp = sizeof(DefineLang) + def->MetaLen;
		free(def);
		return temp;
	}
	else if (StrEq(DEFSRC) == 0)
	{
		DefineSrc* def = node->ValueOrPointer;
		node->ValueOrPointer = (void*)DataLoc;

		if (fwrite(node, sizeof(ProjNode), 1, file) == 0) return 0;
		fseek(file, DataLoc, SEEK_SET);

        for (int i = 0; i < def->FileCount; ++i)
        {
            for (int j = 0; j < map->KvCount; ++j)
            {
                if (map->items[j].oldPtr == def->SrcFiles[i])
                {
                    def->SrcFiles[i] = map->items[j].newPtr;
                }
            }
        }

		if (fwrite(def, sizeof(DefineSrc) + (sizeof(char*) * def->FileCount), 1, file) == 0) return 0;

		u64 temp = sizeof(DefineSrc) + (sizeof(char*) * def->FileCount);
		free(def);
		return temp;
	}
	else if (StrEq(DEFCOMP) == 0)
	{
		DefineCompiler* def = node->ValueOrPointer;
		node->ValueOrPointer = (void*)DataLoc;

        for (int i = 0; i < def->ArgCount; ++i)
        {
            for (int j = 0; j < map->KvCount; ++j)
            {
                if (map->items[j].oldPtr == def->ArgMap[i].Arg)
                {
                    def->ArgMap[i].Arg = map->items[j].newPtr;
                }
            }
        }

		if (fwrite(node, sizeof(ProjNode), 1, file) == 0) return 0;

		fseek(file, DataLoc, SEEK_SET);

		if (fwrite(def, sizeof(DefineCompiler), 1, file) == 0) return 0;
		u64 accum = 0;
		for (int i = 0; i < def->ArgCount; ++i)
		{
			if (accum += fwrite(&def->ArgMap[i], sizeof(CompArgKV) + def->ArgMap[i].ArgLen, 1, file) == 0) return 0;
		}

		free(def);
		return sizeof(DefineCompiler) + accum;
	}
    else if (StrEq(NAME) == 0)
    {
        for (int j = 0; j < map->KvCount; ++j)
        {
            if (map->items[j].oldPtr == node->ValueOrPointer)
            {
                node->ValueOrPointer = map->items[j].newPtr;
            }
        }

        return fwrite(node, sizeof(ProjNode), 1, file);
    }
	return -1;
}

ProjNode ReadNode(const ProjNode* node, FILE* file, ArenaPtrMap* map)
{
    ProjNode retnode;
    if ((u64) node->ValueOrPointer > (u64)INT_MAX)
    {
        goto error;
    }
	if (StrEq(AUTOSRC) == 0)
	{
		return *node;
	}
	else if (StrEq(RECURSE) == 0)
	{
		return *node;
	}
	else if (StrEq(DEFLANG) == 0)
	{
		fseek(file, (long) node->ValueOrPointer, SEEK_SET);

		DefineLang* def = malloc(sizeof(DefineLang));
		if (fread(def, sizeof(DefineLang), 1, file) == 0)
        {
			free(def);
            goto error;
        }

        if (def->MetaLen > 0)
        {
           void* temp =  realloc(def, sizeof(DefineLang) + def->MetaLen);
           if (temp == NULL)
           {
               free(def);
               goto error;
           }
           def = temp;
           if (fread(def->Metadata, def->MetaLen, 1, file) == 0)
           {
               free(def);
               goto error;
           }
        }

		strcpy(retnode.Name, node->Name);
		retnode.ValueOrPointer = def;

		return retnode;
	}
	else if (StrEq(DEFSRC) == 0) //later
	{
		fseek(file, (long) node->ValueOrPointer, SEEK_SET);

		DefineSrc* def = malloc(sizeof(DefineSrc));
		if (fread(def, sizeof(DefineSrc), 1, file) == 0)
        {
            free(def);
            goto error;
        }

        void* temp = realloc(def, sizeof(DefineSrc) + (def->FileCount * sizeof(char*)));
        if (temp == NULL)
        {
            free(def);
            goto error;
        }
        def = temp;

        for (int i = 0; i < def->FileCount; ++i)
        {
            for (int j = 0; j < map->KvCount; ++j)
            {
                if (def->SrcFiles[i] == map->items[j].oldPtr)
                {
                    def->SrcFiles[i] = map->items[j].newPtr;
                }
            }
        }

		strcpy(retnode.Name, node->Name);
		retnode.ValueOrPointer = def;

		return retnode;
	}
    else if (StrEq(DEFCOMP) == 0)
    {
        fseek(file, (long) node->ValueOrPointer, SEEK_SET);

        DefineCompiler* def = malloc(sizeof(DefineCompiler));
        if (fread(def, sizeof(DefineCompiler), 1, file) == 0)
        {
            free(def);
            goto error;
        }

        if (def->ArgCount > 0)
        {
            void* temp;
            u32 accum = sizeof(DefineCompiler);
            for (int i = 0; i < def->ArgCount; ++i)
            {
                accum += sizeof(CompArgKV);
                temp = realloc(def, accum);
                if (temp == NULL)
                {
                    free(def);
                    goto error;
                }
                def = temp;

                fread((def + accum - sizeof(CompArgKV)), sizeof(CompArgKV), 1, file);

                if (def->ArgMap[i].ArgLen > 0)
                {
                    accum += def->ArgMap[i].ArgLen;
                    temp = realloc(def, accum);
                    if (temp == NULL)
                    {
                        free(def);
                        goto error;
                    }
                    def = temp;

                    fread((def + accum - def->ArgMap[i].ArgLen), def->ArgMap[i].ArgLen, 1, file);
                }
            }
        }


        strcpy(retnode.Name, node->Name);
        retnode.ValueOrPointer = def;

        return retnode;
    }
    else if (StrEq(NAME) == 0)
    {
        for (int j = 0; j < map->KvCount; ++j)
        {
            if (map->items[j].oldPtr == node->ValueOrPointer)
            {
                retnode.ValueOrPointer = map->items[j].newPtr;
            }
        }

        strcpy(retnode.Name, node->Name);
        return retnode;
    }

    error:
	strcpy(retnode.Name, "ERROR");
	return retnode;
}

i32 LoadProject(const char* path, Maple_Project* proj)
{
	FILE* file = fopen(path, "rb");
	if (file == NULL) return -1;

	if (fread(proj, sizeof(Maple_Project), 1, file) == 0) return -1;

	void* temp = realloc(proj, sizeof(Maple_Project) + (sizeof(ProjNode) * proj->NodeCount));
	if (temp == NULL) return -1;
	proj = temp;

	if (fread(proj->nodes, sizeof(ProjNode), proj->NodeCount, file) < proj->NodeCount) return -1;

    proj->arena.Data = malloc(proj->arena.DataSize);
    if (fread(proj->arena.Data, proj->arena.DataSize, 1, file) == 0) return -1;

    proj->arena.Pointers = malloc(sizeof(void*) * proj->arena.PtrCount);
    if (fread(proj->arena.Pointers, sizeof(void*), proj->arena.PtrCount, file) < proj->arena.PtrCount) return -1;

    proj->arena.FreedPtrs = malloc(proj->arena.PtrCount);
    if (fread(proj->arena.FreedPtrs, sizeof(u8), proj->arena.PtrCount, file) < proj->arena.PtrCount) return -1;

    ArenaPtrMap* map = malloc(sizeof(ArenaPtrMap) + (sizeof(ArenaPtrMapKV) * proj->arena.PtrCount));

    for (int i = 0; i < proj->arena.PtrCount; ++i)
    {
        map->items[i].oldPtr = proj->arena.Pointers[i];
        map->items[i].newPtr = proj->arena.Data + (u64)(proj->arena.Pointers[i] - (sizeof(Maple_Project) + (sizeof(ProjNode) * proj->NodeCount)));
        proj->arena.Pointers[i] = map->items[i].newPtr;
    }

	for (int i = 0; i < proj->NodeCount; ++i)
	{
		ProjNode tempnode = ReadNode(&proj->nodes[i], file, map);
		if (strcmp(tempnode.Name, "ERROR") == 0) return -1;
		proj->nodes[i] = tempnode;
	}
	return 0;
}

i32 SaveProject(const char* path, Maple_Project* proj)
{
	FILE* file = fopen(path, "wb");
	if (file == NULL) return -1;

    u64 DataLoc = sizeof(Maple_Project) + (proj->NodeCount * sizeof(ProjNode));
    ArenaPtrMap* map = Arena_Compact(&proj->arena);

    for (int i = 0; i < proj->arena.PtrCount; ++i)
    {
        for (int j = 0; j < map->KvCount; ++j)
        {
            if (proj->arena.Pointers[i] == map->items[j].newPtr)
            {
                map->items[j].newPtr = (void*)((void*)proj->arena.Data - proj->arena.Pointers[i] + DataLoc);
                proj->arena.Pointers[i] = map->items[j].newPtr;
            }
        }
    }

    u64 old = ftell(file);
    fseek(file, DataLoc, SEEK_SET);
    if (fwrite(proj->arena.Data, proj->arena.DataSize, 1, file) == 0)
    {
        fclose(file);
        return -1;
    }
    proj->arena.Data = (u8*)DataLoc;
    DataLoc += proj->arena.DataSize;
    if (fwrite(proj->arena.Pointers, proj->arena.PtrCount * sizeof(void*), 1, file) == 0)
    {
        fclose(file);
        return -1;
    }
    proj->arena.Pointers = (void**) DataLoc;
    DataLoc += proj->arena.PtrCount * sizeof(void*);
    if (fwrite(proj->arena.FreedPtrs, proj->arena.PtrCount * sizeof(u8), 1, file) == 0)
    {
        fclose(file);
        return -1;
    }
    proj->arena.FreedPtrs = (void*)DataLoc;
    DataLoc += proj->arena.PtrCount * sizeof(u8);
    fseek(file, old, SEEK_SET);

	if (fwrite(proj, sizeof(Maple_Project), 1, file) == 0)
	{
		fclose(file);
		return -1;
	}


	for (int i = 0; i < proj->NodeCount; ++i)
	{
		u64 loc = ftell(file);
		DataLoc += WriteNode(&proj->nodes[i], file, DataLoc, map);
		fseek(file, loc + sizeof(ProjNode), SEEK_SET);
	}
    free(map);
	fclose(file);
	return 0;
}



void SetNode(ProjNode* node, const char* Name, void* ValueOrPointer)
{
	strcpy(node->Name, Name);
	node->ValueOrPointer = ValueOrPointer;
}

i32 GenerateNewProjectFromDefaults(const char* name) //redo
{
	LocalProject = malloc(sizeof(Maple_Project) + (sizeof(ProjNode) * 3));
	memset(LocalProject, 0 , sizeof(Maple_Project) + (sizeof(ProjNode) * 3));
	strcpy(LocalProject->MAGIC, "MAPLE");
	LocalProject->NodeCount = 3;
	LocalProject->reserved = 0;

    Arena_init(&LocalProject->arena);
    char* nam = Arena_alloc(&LocalProject->arena, strlen(name));
    strcpy(nam, name);

	SetNode(&LocalProject->nodes[0], AUTOSRC, (void*)1);
	SetNode(&LocalProject->nodes[1], RECURSE, (void*)1);
    SetNode(&LocalProject->nodes[2], NAME, nam);
	//add more once src and compiler shit works
	return 0;
}

