; ===============================================================
; HEADERS.ASM for TI-83 Plus Applications
; ---------------------------------------------------------------
; This template file is designed for use with Latenite. It 
; requires the Brass Z80 assembler.
;
; It takes two environment variables; shell and platform. It also
; assumes the following directory structure:
;
; .\Page0.asm            <- First page of source code
; .\Page1.asm            <- Optional second page of source code
; .\Page2.asm            <- Optional third page of source code
;                           (etc)
; .\Includes\Headers.asm <- This file
; .\Includes\?.inc       <- Various include files...
; .\Includes\?.lbl       <- ...and label files.
;
; You do NOT assemble Page0.asm directly - you must assemble
; this file, which will in turn assemble Page0.asm
; ===============================================================

app_page_count .equ 1 ; Change this to the number of pages

; ===============================================================
; General assembler directives
; ===============================================================

.binarymode ti8xapp              ; TI-83+ Application

.variablename [%PROJECT_BINARY%] ; Application name

.inclabels "ti8x.lbl"            ; Label file containing equates

.deflong bjump(label)            ; Jump macro
    call BRT_JUMP0
    .dw label
.enddeflong

.deflong bcall(label)            ; Call macro
    rst rBR_CALL
    .dw label
.enddeflong

.for app_page, 0, app_page_count - 1
.defpage app_page, 16*1024, $4000
.loop

.page 0     ; Start page 0
.block 128  ; Reserve 128 bytes for header
    jp Main ; Jump past the branch table
        
;=======================================================
; Jump Table
;=======================================================

; Use the .branch directive to create a jump table for
; off-page calls, e.g.:

; .branch Page1_Routine1
; .branch Page1_Routine2
; .branch Page2_Routine1
; .branch Page2_Routine2

;=======================================================
; End Jump Table
;=======================================================

.for app_page, 0, app_page_count - 1
.page app_page
.include "..\Page" + app_page + ".asm"
.loop