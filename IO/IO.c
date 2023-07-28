#include "IO.h"
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <assert.h>
#include "../base.h"

u64 IO_flength(FILE* file)
{
	u64 old = ftell(file);

	fseek(file, 0, SEEK_END);
	u64 ret = ftell(file);

	fseek(file, old, SEEK_SET);

	return ret;
}

void IO_SanitizePath(char* path)
{
	for (int i = 0; i < strlen(path); ++i)
	{
		if (path[i] == PATH_SEPARATOR)
		{
			path[i] = '/';
		}
	}
}

void IO_UnsanitizePath(char* path)
{
	for (int i = 0; i < strlen(path); ++i)
	{
		if (path[i] == '/')
		{
			path[i] = PATH_SEPARATOR;
		}
	}
}

typedef struct {
	FILE* file;
	FILE* bakfile;
} FileMapKV;

typedef struct {
	FileMapKV files[1024];
} FileMap;

static FileMap fmap;

void IO_Init()
{
	memset(&fmap, 0, sizeof(FileMap));
}

static void AddFmap(FileMapKV kv)
{
	for (int i = 0; i < 1024; ++i) {
		if (fmap.files[i].file != NULL)
		{
			fmap.files[i] = kv;
			return;
		}
	}
	assert(0);
}

static void RemoveFmap(FILE* file)
{
	for (int i = 0; i < 1024; ++i) {
		if (fmap.files[i].file == file)
		{
			fmap.files[i].file = NULL;
			fclose(fmap.files[i].bakfile);
			fmap.files[i].bakfile = NULL;
			return;
		}
	}
	assert(0);
}

static FILE* GetFmap(FILE* file)
{
	for (int i = 0; i < 1024; ++i) {
		if (fmap.files[i].file == file)
		{
			return fmap.files[i].bakfile;
		}
	}
	assert(0);
}

FILE* IO_SafeOpen(char* path, const char* mode)
{
	IO_SanitizePath(path);

	char* bakpath = strcat(path, ".bak");

	FILE* file = fopen(path, mode);
	FILE* bakfile = tmpfile();

	u64 len = IO_flength(file);

	u8 data[len];
	if (fread(data, len, 1, file) == 0) return NULL;
	if (fwrite(data, len, 1, bakfile) == 0) return NULL;

	AddFmap((FileMapKV){file, bakfile});

	free(bakpath);

	IO_UnsanitizePath(path);
	return file;
}

void IO_SafeClose(FILE* file)
{
	RemoveFmap(file);
	fclose(file);
}

void IO_RevertFile(FILE* file)
{
	FILE* bakfile = GetFmap(file);
	u64 len = IO_flength(bakfile);

	fseek(file, 0, SEEK_SET);
	fseek(bakfile, 0, SEEK_SET);

	u8 data[len];
	while (!feof(bakfile))
	{
		fread(data + ftell(bakfile), 1, len, bakfile);
	}
	u64 i = 0;
	while (i < len)
	{
		i += fwrite(data + i, 1, len, file);
	}
	fclose(bakfile);
}

void IO_PANIC()
{
	for (int i = 0; i < 1024; ++i) {
		if (fmap.files[i].file != NULL)
		{
			IO_RevertFile(fmap.files[i].file);
			fclose(fmap.files[i].file);
		}
	}
}