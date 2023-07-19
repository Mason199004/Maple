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
	else if (StrEq(DEFSRC) == 0)
	{
		fseek(file, (long) node->ValueOrPointer, SEEK_SET);

		DefineLang* def = malloc(sizeof(DefineSrc));
		if (fread(def, sizeof(DefineSrc), 1, file) == 0)
        {
            free(def);
            goto error;
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

    error:
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



