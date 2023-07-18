#include <string.h>
#include <malloc.h>
#include "Project.h"
static Maple_Project GlobalProject;
static Maple_Project LocalProject;

#define StrEq(X) strcmp(node->Name, X)

u64 WriteNode(ProjNode node, FILE* file)
{
	return 0;
}

ProjNode ReadNode(const ProjNode* node, FILE* file)
{
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
		fread(def, sizeof(DefineLang), 1, file);

		ProjNode retnode;
		strcpy(retnode.Name, node->Name);
		retnode.ValueOrPointer = def;

		return retnode;
	}
	else if (StrEq(DEFSRC) == 0)
	{
		fseek(file, (long) node->ValueOrPointer, SEEK_SET);

		DefineLang* def = malloc(sizeof(DefineSrc));
		fread(def, sizeof(DefineSrc), 1, file);

		ProjNode retnode;
		strcpy(retnode.Name, node->Name);
		retnode.ValueOrPointer = def;

		return retnode;
	}
	ProjNode retnode;
	strcpy(retnode.Name, "ERROR");
	return retnode;
}

i32 LoadProject(const char* path)
{
	return 0;
}

i32 SaveProject(const char* path)
{
	return 0;
}



