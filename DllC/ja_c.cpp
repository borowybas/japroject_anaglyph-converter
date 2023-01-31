#pragma once
#include <iostream>
#include "pch.h"
/**
 * Anaglyph convertion from sterescopic pair
 *
 * Program accepts two images in formats .jpg, .jpeg, .png. User has a posibility to choose a method of convertion algorythm
 * preformed by ASM dynamic lik library and C++ dynamic link library.There is a time measurment of methods preformed in dynamic
 * link libraries. For the resulting image, we take the red color component from the image for the left eye and the green and
 * blue components from the image for the right eye.
 *
 * @author Barbara Borowy
 * @version 1.0
 * @date 31.01.2023 semester 5, academic year 2022/2023
 */


/**
* Procedure convertCPP that performs algorthm of convering two images to anaglyph in cpp
*
* Input parameters:
*       unsigned char leftRgbValues[] - byte array of gbr values of every pixel in left image
*		unsigned char rightRgbValues[] - byte array of gbr values of every pixel in right image
*		int bytesLength - length of byte arrays of images
* Output parameters:
*       output image saved in rightRgbValues[]
*/
extern "C" void __declspec(dllexport) convertCPP(unsigned char leftRgbValues[], unsigned char rightRgbValues[], int bytesLength) {
    for (int counter = 0; counter < bytesLength; counter += 3)
    {
        //rightRgbValues[counter] = rightRgbValues[counter];                  //blue value
        //rightRgbValues[counter + 1] = rightRgbValues[counter + 1];          //green value
        rightRgbValues[counter + 2] = leftRgbValues[counter + 2];           //red value
    }
}//bgr