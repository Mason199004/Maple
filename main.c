#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include "base.h"
#include "Proj/Project.h"

void printVersion();
i32 newProject(char* name);

static char* commands[] = {"new", "build", "add"};

int main(int argc, char** argv)
{
	if (argc == 1)
	{
		printVersion();
		exit(0);
	}

	if (strcmp(argv[1], commands[0]) == 0)
	{
		if (argc < 3)
		{
			puts("No name given for new project!");
			exit(1);
		}
		if (newProject(argv[2]))
		{
			puts("An error occurred while creating new project.");
			exit(1);
		}
	}
	return 0;
}

void printVersion()
{
	puts("Maple build system v" MAPLE_VERSION " : " __DATE__ );
}

i32 newProject(char* name)
{
	GenerateNewProjectFromDefaults();
	char cwd[PATH_MAX];
	if (getcwd(cwd, sizeof(cwd)) == NULL) return -1;
	u64 len = strlen(name);
	char path[PATH_MAX + len];

	sprintf(path, "%s%c%s", cwd, PATH_SEPARATOR, name);

	SaveLocalProject(path);
	return 0;
}
