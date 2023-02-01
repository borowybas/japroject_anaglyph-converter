;***************************************************
;* Anaglyph convertion from sterescopic pair
;*
;* Program accepts two images in formats .jpg, .jpeg, .png. User has a posibility to choose a method of convertion algorythm 
;* preformed by ASM dynamic lik library and C++ dynamic link library.There is a time measurment of methods preformed in dynamic 
;* link libraries. For the resulting image, we take the red color component from the image for the left eye and the green and 
;* blue components from the image for the right eye.
;*
;* @author Barbara Borowy
;* @version 1.0
;* @date 31.01.2023 semester 5, academic year 2022/2023
;**************************************************

.code

;**************************************************
;*
;* Procedure convertASM that performs algorthm of convering two images to anaglyph in asm
;*
;* Input parameters:
;*		RCX - byte array of gbr values of every pixel in left image (byte[] leftRgbValues,	= RCX red)
;*		RDX - byte array of gbr values of every pixel in right image (byte[] rightRgbValues, = RDX cyjan return)
;*		R8 - length of byte arrays of images (int bytesLength		=R88)
;*
;* Output parameters:
;*		RDX - output byte array
;*
;**************************************************

convertASM proc
mov r10, 0		;set the offset pointer to 0

MainLoop:
inc r10			;increment offset
dec r8			;decrement number of left bytes to read
jz myend		;jump to end if end of bytes

inc r10			;increment offset
dec r8			;decrement number of left bytes to read
jz myend

mov al, byte ptr [rcx+r10]
mov byte ptr [rdx+r10], al ;write left image red value of pixel to output image
inc r10			;increment offset
dec r8			;decrement number of left bytes to read

jnz MainLoop
myend:
ret				;return from procedure
convertASM endp


end