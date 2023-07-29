#include <stdlib.h>
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
	char* path = getenv("HOME");
#endif
	i32 ret = SaveProject(path, GlobalProject);
	free(path);
	return ret;
}

i32 LoadGlobalProject()
{
#ifdef __WIN32__
	char* path = getenv("APPDATA");
#else
	char* path = getenv("HOME");
#endif
	i32 ret = LoadProject(path, GlobalProject);
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

ProjNode* GetNode(const char* name)
{
	//TODO: Implement
}