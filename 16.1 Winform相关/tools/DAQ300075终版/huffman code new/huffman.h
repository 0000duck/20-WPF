#ifndef _RIGOL_FOR_INTER_AND_SAMP_H_
#define _RIGOL_FOR_INTER_AND_SAMP_H_
typedef struct Node
{
	int weight;                //权值  
	int parent;                //父节点的序号，为-1的是根节点  
	int lchild, rchild;         //左右孩子节点的序号，为-1的是叶子节点  
}HTNode, * HuffmanTree;          //用来存储赫夫曼树中的所有节点  
typedef char** HuffmanCode;    //用来存储每个叶子节点的赫夫曼编码  

#define maxSize 256
int N = 4000042;

void select_minium(HuffmanTree HT, int k, int& min1, int& min2);
int min(HuffmanTree HT, int k);
void HuffmanCoding1(HuffmanTree HT, HuffmanCode& HC, int n);
void HuffmanCoding2(HuffmanTree HT, HuffmanCode& HC, int n);
int countWPL1(HuffmanTree HT, int n);
int countWPL2(HuffmanTree HT, int n);
/*
赫夫曼树的存储结构，它也是一种二叉树结构，
这种存储结构既适合表示树，也适合表示森林。
*/


#ifndef _RIGOL_DLL_API
#define _RIGOL_DLL_API extern "C" _declspec(dllexport) 
#else
#endif
_RIGOL_DLL_API void create_HuffmanTree(int* wet, int n);
_RIGOL_DLL_API int HuffmanDeCoding1(unsigned char* pu8DataBuff, int u32DataLength, const char* fileout);
#endif