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
	//TODO finish
}