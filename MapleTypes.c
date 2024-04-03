#include "MapleTypes.h"
#include <string.h>

BOOL maple_validate_printable_string(LSTR* str) //todo: make better, possibly mandate ascii for certain things
{
    for (int i = 0; i < str->size; ++i)
    {
        if (str->str[i] == 0x1b)
        {
            return false;
        }
    }
    return true;
}

BOOL maple_lstrcmp(LSTR* a, LSTR* b)
{
    if (a->size == b->size)
    {
        return !memcmp(a->str, b->str, a->size);
    }
    return false;
}