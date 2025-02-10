#include "../MapleTypes.h"
#ifndef MAPLE_MAPLEPROJ_H
#define MAPLE_MAPLEPROJ_H

/*
 * Maple Build System binary configuration format; build.maple
 *  HEADER:
 *    5 byte magic ascii"MAPLE"
 *    Property Descriptor Table: describes all properties used by current project, including those added by plugins
 *  PROPERTY DATA:
 *    U64 length of section
 *    [list of properties]
 *  DATA STORE: \\heap like structure containing any data that could not be contained in a property
 *    U64 length of section
 *    [data]
 */

/// probably going to move this into a core plugin, might be interesting to have the internal prop
/// representation be its own thing and then any plugin can serialize them however they want

#endif //MAPLE_MAPLEPROJ_H
