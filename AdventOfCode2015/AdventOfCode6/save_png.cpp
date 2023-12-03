#include "save_png.h"

#include <Windows.h>
#include <wincodec.h>

#pragma comment(lib, "windowscodecs.lib")

#define CHECKHR(a) \
{ \
	const HRESULT hr = (a); \
	if(!SUCCEEDED(hr)) DebugBreak(); \
}

void save_png(const std::string & filename, const void * pixels, const int width, const int height)
{
	std::wstring wfilename(filename.begin(), filename.end());

	IWICImagingFactory * gpiImagingFactory = NULL;
	IWICStream * piStream = NULL;
	IWICBitmapEncoder * piEncoder = NULL;
	IWICBitmapFrameEncode * piBitmapFrame = NULL;
	IPropertyBag2 * piPropertyBag = NULL;

	CHECKHR(CoInitialize(0));
	CHECKHR(CoCreateInstance(CLSID_WICImagingFactory, NULL, CLSCTX_INPROC_SERVER, IID_IWICImagingFactory, (LPVOID*)&gpiImagingFactory));

	CHECKHR(gpiImagingFactory->CreateStream(&piStream));
	CHECKHR(piStream->InitializeFromFilename(wfilename.c_str(), GENERIC_WRITE));
	CHECKHR(gpiImagingFactory->CreateEncoder(GUID_ContainerFormatPng, NULL, &piEncoder));

	CHECKHR(piEncoder->Initialize(piStream, WICBitmapEncoderNoCache));
	CHECKHR(piEncoder->CreateNewFrame(&piBitmapFrame, &piPropertyBag));

	WICPixelFormatGUID pixelFormat = GUID_WICPixelFormat24bppBGR;

	CHECKHR(piBitmapFrame->Initialize(piPropertyBag));
	CHECKHR(piBitmapFrame->SetPixelFormat(&pixelFormat));

	CHECKHR(piBitmapFrame->SetSize(width, height));
	CHECKHR(piBitmapFrame->WritePixels(height, width * 3, width * height * 3, (BYTE*)pixels));

	CHECKHR(piBitmapFrame->Commit());
	CHECKHR(piEncoder->Commit());

	CHECKHR(piBitmapFrame->Release());
	CHECKHR(piEncoder->Release());
	CHECKHR(piStream->Release());
	CHECKHR(gpiImagingFactory->Release());
}
