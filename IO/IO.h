#ifndef MAPLE_IO_H
#define MAPLE_IO_H
#include <stdio.h>
#include "../base.h"

u64 IO_flength(FILE* file);

void IO_SanitizePath(char* path);

void IO_UnsanitizePath(char* path);

FILE* IO_SafeOpen(char* path, const char* mode);

void IO_SafeClose(FILE* file);

void IO_RevertFile(FILE* file);

void IO_PANIC();

#endif //MAPLE_IO_H
