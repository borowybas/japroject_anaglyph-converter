; static extern int convertASM(
;byte[] leftRgbValues,	= RCX red
;byte[] rightRgbValues, = RDX cyjan return
;int bytesLength		=R88 );
.code
convertASM proc
mov r10, 0		;set the offset pointer to 0

MainLoop:
inc r10
dec r8
jz myend

inc r10
dec r8
jz myend

mov al, byte ptr [rcx+r10]
mov byte ptr [rdx+r10], al
inc r10
dec r8

jnz MainLoop
myend:
ret
convertASM endp


end