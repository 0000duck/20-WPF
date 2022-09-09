#ifndef _RIGOL_FOR_INTER_AND_SAMP_H_
#define _RIGOL_FOR_INTER_AND_SAMP_H_
typedef struct Node
{
	int weight;                //Ȩֵ  
	int parent;                //���ڵ����ţ�Ϊ-1���Ǹ��ڵ�  
	int lchild, rchild;         //���Һ��ӽڵ����ţ�Ϊ-1����Ҷ�ӽڵ�  
}HTNode, * HuffmanTree;          //�����洢�շ������е����нڵ�  
typedef char** HuffmanCode;    //�����洢ÿ��Ҷ�ӽڵ�ĺշ�������  

#define maxSize 256
int N = 4000042;

void select_minium(HuffmanTree HT, int k, int& min1, int& min2);
int min(HuffmanTree HT, int k);
void HuffmanCoding1(HuffmanTree HT, HuffmanCode& HC, int n);
void HuffmanCoding2(HuffmanTree HT, HuffmanCode& HC, int n);
int countWPL1(HuffmanTree HT, int n);
int countWPL2(HuffmanTree HT, int n);
/*
�շ������Ĵ洢�ṹ����Ҳ��һ�ֶ������ṹ��
���ִ洢�ṹ���ʺϱ�ʾ����Ҳ�ʺϱ�ʾɭ�֡�
*/


#ifndef _RIGOL_DLL_API
#define _RIGOL_DLL_API extern "C" _declspec(dllexport) 
#else
#endif
_RIGOL_DLL_API void create_HuffmanTree(int* wet, int n);
_RIGOL_DLL_API int HuffmanDeCoding1(unsigned char* pu8DataBuff, int u32DataLength, const char* fileout);
#endif