#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include "base.h"
#include "Proj/ProjectHandler.h"

void printVersion();
i32 newProject(char* name);
i32 buildProject(char* path);

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

	if (strcmp(argv[1], commands[1]) == 0)
	{
		char path[PATH_MAX];
		memset(path, 0, PATH_MAX);
		getcwd(path, PATH_MAX);

		if (path[strlen(path)] != '/')
		{
			path[strlen(path)] = '/';
		}

		if (buildProject(path))
		{
			fprintf(stderr, "%s", "An error has occurred while building\n");
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
	GenerateNewProjectFromDefaults(name);
	char cwd[PATH_MAX];
	if (getcwd(cwd, sizeof(cwd)) == NULL) return -1;
	u64 len = strlen(name);
	char path[PATH_MAX + len];

	if (strlen(cwd) + len > PATH_MAX) return -1;
	sprintf(path, "%s%cbuild.maple", cwd, PATH_SEPARATOR);

	SaveLocalProject(path);
	return 0;
}

i32 buildProject(char* path)
{
	if (SetupEnv(path))
	{
		return -1;
	}

	return 0;
}
