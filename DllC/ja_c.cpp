#pragma once
#include <iostream>
#include "pch.h"

extern "C" void __declspec(dllexport) convertCPP(unsigned char leftRgbValues[], unsigned char rightRgbValues[], int bytesLength) {
    for (int counter = 0; counter < bytesLength; counter += 3)
    {
        rightRgbValues[counter] = rightRgbValues[counter];
        rightRgbValues[counter + 1] = rightRgbValues[counter + 1];
        rightRgbValues[counter + 2] = leftRgbValues[counter + 2];
    }
}//bgr