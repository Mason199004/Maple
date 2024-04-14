#ifndef MAPLE_LOGGING_H
#define MAPLE_LOGGING_H

#include "../MapleTypes.h"

void maple_system_log_error(const char* prefix, LSTR error);

void maple_system_log_warning(const char* prefix, LSTR warn);

void maple_system_log_info(const char* prefix, LSTR info);

[[noreturn]] void maple_system_panic(const char* prefix, const char* message);

#endif //MAPLE_LOGGING_H
