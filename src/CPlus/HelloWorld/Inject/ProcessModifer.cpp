#include "../stdafx.h"
#include "ProcessModifer.h"
#include <Windows.h>
#include <stdio.h>


ProcessModifer::ProcessModifer(char * name)
{
}


ProcessModifer::~ProcessModifer()
{
}

//����window ���ش��ھ��
HWND ProcessModifer::FindWindow_Ex(LPCTSTR ipClassName,LPCTSTR ipWindowName)
{
	return ::FindWindow(ipClassName, ipWindowName);
}
//ͨ�����ھ����ȡ�������ڽ���ID���߳�ID
DWORD ProcessModifer::GetWindowThreadProcessId_Ex(HWND hWnd, LPDWORD &lpdwProcessId)
{
	return ::GetWindowThreadProcessId(hWnd, lpdwProcessId);
}

//���Ѵ��ڵĽ��̣����ؽ��о��
HANDLE ProcessModifer::OpenProcess_Ex(DWORD dwDesiredAccess, BOOL bInheritHandle, DWORD dwProcessId)
{
	return ::OpenProcess(dwDesiredAccess, bInheritHandle, dwProcessId);
}

//��������д������
bool ProcessModifer::WriteProcessMemory_Ex(HANDLE hProcess, LPVOID lpBaseAddress, LPVOID lpBuffer, DWORD nSize, LPDWORD lpNumberOfBytesWritten)
{
	return ::WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, nSize, lpNumberOfBytesWritten);
}

void ProcessModifer::Begin()
{
	LPCTSTR name = LPCTSTR("test.exe");
	HWND h = FindWindow_Ex(NULL, name);
	LPDWORD lpid;
	DWORD res = GetWindowThreadProcessId_Ex(h, lpid);
	HANDLE pidh;
	pidh = OpenProcess_Ex(PROCESS_ALL_ACCESS, FALSE, *lpid);
	if (pidh == 0) {
		printf("�򿪽���ʧ��");
		return;
	}
	else {
		printf("�򿪽��̳ɹ�");
		DWORD hp = 300;
		LPCVOID addr = (LPCVOID)0xFFFF;
		DWORD res2 = WriteProcessMemory_Ex(pidh, (LPVOID)addr, &hp, 4, 0);
	}
}

