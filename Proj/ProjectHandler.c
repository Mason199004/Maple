#include <stdlib.h>
#include <string.h>
#include "ProjectHandler.h"

i32 SaveLocalProject(const char* path)
{
	return SaveProject(path, LocalProject);
}

i32 LoadLocalProject(const char* path)
{
	return LoadProject(path, LocalProject);
}

i32 SaveGlobalProject()
{
#ifdef __WIN32__
	char* path = getenv("APPDATA");
#else
	char* path = getenv("XDG_CONFIG_HOME");
#endif
	char full[strlen(path) + strlen("config.maple") + 2];
	if (path[strlen(path)] != PATH_SEPARATOR)
	{
		sprintf(full, "%s%c%s", path, PATH_SEPARATOR, "config.maple");
	}
	else
	{
		sprintf(full, "%s%s", path, "config.maple");
	}

	i32 ret = SaveProject(full, GlobalProject);
	free(path);
	return ret;
}

i32 LoadGlobalProject()
{
#ifdef __WIN32__
	char* path = getenv("APPDATA");
#else
	char* path = getenv("XDG_CONFIG_HOME");
#endif
	char full[strlen(path) + strlen("config.maple") + 2];
	if (path[strlen(path)] != PATH_SEPARATOR)
	{
		sprintf(full, "%s%c%s", path, PATH_SEPARATOR, "config.maple");
	}
	else
	{
		sprintf(full, "%s%s", path, "config.maple");
	}

	i32 ret = LoadProject(full, GlobalProject);
	free(path);
	return ret;
}

i32 SaveHiddenProject(const char* path)
{
	return SaveProject(path, HiddenProject);
}

i32 LoadHiddenProject(const char* path)
{
	return LoadProject(path, HiddenProject);
}

ProjNode* GetNodeFromProj(Maple_Project* proj, const char* name)
{
	for (int i = 0; i < proj->NodeCount; ++i)
	{
		if (strcmp(proj->nodes[i].Name, name) == 0)
		{
			return &proj->nodes[i];
		}
	}
	return NULL;
}

ProjNode* GetNode(const char* name)
{
	if (GetNodeFromProj(HiddenProject, name) != NULL)
	{
		return GetNodeFromProj(GlobalProject, name);
	}
	else
	{
		ProjNode* ret;
		if ((ret = GetNodeFromProj(LocalProject, name)) != NULL)
		{
			return ret;
		}
		else
		{
			return GetNodeFromProj(GlobalProject, name);
		}
	}
}