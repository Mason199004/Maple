#ifndef MAPLE_ARENA_H
#define MAPLE_ARENA_H
#include "../base.h"

typedef struct {
	u64 DataSize;
	u64 PtrCount;
	u8* FreedPtrs;
	void** Pointers;
	u8* Data;
} Arena;

typedef struct {
	void* oldPtr;
	void* newPtr;
} ArenaPtrMapKV;

typedef struct {
	u64 KvCount;
	ArenaPtrMapKV items[];
} ArenaPtrMap;

void* Arena_alloc(Arena* arena, u64 size);

i32 Arena_free(Arena* arena, void* ptr);

void* Arena_realloc(Arena* arena, void* ptr, u64 newSize);

ArenaPtrMap* Arena_Compact(Arena* arena);

void Arena_init(Arena* arena);

#endif //MAPLE_ARENA_H
