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

ProjNode* GetNodeFromProj(Maple_Project* proj, const char* name, i32 NodeNum)
{
	i32 j = 0;
	for (int i = 0; i < proj->NodeCount; ++i)
	{
		if (strcmp(proj->nodes[i].Name, name) == 0)
		{
			if (j == NodeNum)
			{
				return &proj->nodes[i];
			}
			j++;
		}
	}
	return NULL;
}

i32 CountNodes(const char* name)
{
	i32 ret = 0;
	for (int i = 0; i < LocalProject->NodeCount; ++i)
	{
		if (strcmp(LocalProject->nodes[i].Name, name) == 0)
		{
			ret++;
		}
	}
	return ret;
}

ProjNode* GetNode(const char* name, i32 NodeNum)
{
	if (GetNodeFromProj(HiddenProject, name, NodeNum) != NULL)
	{
		return GetNodeFromProj(GlobalProject, name, NodeNum);
	}
	else
	{
		ProjNode* ret;
		if ((ret = GetNodeFromProj(LocalProject, name, NodeNum)) != NULL)
		{
			return ret;
		}
		else
		{
			return GetNodeFromProj(GlobalProject, name, NodeNum);
		}
	}
}

i32 SetupEnv(const char* ProjDir)
{
	char* proj = strcat(ProjDir, "build.maple");
	char* hidden = strcat(ProjDir, "working/build.maple");
	i32 err = 0;

	err += LoadLocalProject(proj);
	err += LoadGlobalProject();
	err += LoadHiddenProject(hidden);

	if (err)
	{
		return -1;
	}

	free(proj);
	free(hidden);
	return 0;
}