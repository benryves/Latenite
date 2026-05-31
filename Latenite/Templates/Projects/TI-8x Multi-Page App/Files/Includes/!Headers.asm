; ===============================================================
; HEADERS.ASM - Benjamin Ryves 2005-2006
; Edited by Andrew Ho 2006
; ---------------------------------------------------------------
; This template file is designed for use with Latenite. It 
; requires the Brass Z80 assembler.
;
; It takes two environment variables; shell and platform. It also
; assumes the following directory structure:
;
; .\Page0.asm          <- Main program source code
; .\Page1.asm
; ...

; .\Includes\Headers.asm <- This file
; .\Includes\?.inc       <- Various include files...
; .\Includes\?.lbl       <- ...and label files.
;
; You do NOT assemble Pag0.asm directly - you must assemble
; this file, which will in turn assemble Program.asm
;
; Always use bcall(_xxxx) and variants to call TIOS routines.
; This will get expanded to the more usual call _xxxx for non-83+
; calculators.
;
; This Header File has been edited to compile Multi-Page Appications
; for the 83+ series and should not be used for other purposes,
; as it will probably not work correctly.
;
; ===============================================================

; ===============================================================
; General assembler directives
; ===============================================================

.binarymode ti8xapp				; TI-83+ Application

.variablename [%PROJECT_BINARY%]		; Application name (shown in 'Apps' menu) Must be 8 

.inclabels "ti8x.lbl"			; Label file containing equates

.deflong bjump(label)			; Jump macro
    call BRT_JUMP0
    .dw label
.enddeflong

.deflong bcall(label)			; Call macro
    rst rBR_CALL
    .dw label
.enddeflong

.defpage 0, 16*1024, $4000			; Page 0 definition
.defpage 1, 16*1024, $4000			; Page 1 definition

.page 0					; Start page 0
						; <- header is added in here for us.
.block 128					; Advance 128 bytes for header.   
        
 jp Main					; Jump past the branch table
        
;=======================================================
;	Jump Table
;=======================================================

.branch Page1_Main				; Add a branch table item

;=======================================================
;	End Jump Table
;=======================================================

.include "../Page0.asm"			;Page includes. Add more to add more pages

.page 1					;Define Page 1 Start

.include "../Page1.asm"