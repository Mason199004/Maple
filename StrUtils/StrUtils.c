#include "StrUtils.h"
#include "../base.h"
#include <string.h>
#include <malloc.h>
#include <stdio.h>


char* Str_Insert(const char* src, int pos, const char* ToInsert)
{
	u64 len = strlen(src);
	char FirstHalf[len - (len - pos)];
	char SecondHalf[len - pos];

	strncpy(FirstHalf, src, len - (len - pos));
	strncpy(SecondHalf, (src + pos), len - pos);

	char* ret = malloc(len + strlen(ToInsert));
	sprintf(ret, "%s%s%s", FirstHalf, ToInsert, SecondHalf);

	return ret;
}