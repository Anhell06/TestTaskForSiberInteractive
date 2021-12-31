using System.Collections.Generic;
using System.IO;

namespace TestTaskForSiberInteractive
{
    public class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random;
        public string Data;
    }

    public class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(s))
            {
                int index = 0;
                Dictionary<ListNode, int> IDByNodes = new Dictionary<ListNode, int>();

                for (ListNode node = Head; index < Count; node = node.Next, index++)
                {
                    IDByNodes.Add(node, index);
                }

                index = 0;
                for (ListNode node = Head; index < Count; node = node.Next, index++)
                {
                    binaryWriter.Write(IDByNodes[node]);
                    binaryWriter.Write(IDByNodes[node.Random]);
                    if (node.Data != null)
                    {
                        binaryWriter.Write(node.Data);
                    }
                    else
                    {
                        binaryWriter.Write(string.Empty);
                    }
                }
            }
        }

        public void Deserialize(FileStream s)
        {
            using (BinaryReader binaryReader = new BinaryReader(s))
            {
                Count = 0;

                Dictionary<int, ListNode> NodesByID = new Dictionary<int, ListNode>();
                Queue<int> LinkAtRandom = new Queue<int>();
                
                while (binaryReader.PeekChar() != -1)
                {
                    NodesByID[binaryReader.ReadInt32()] = new ListNode();
                    LinkAtRandom.Enqueue(binaryReader.ReadInt32());

                    NodesByID[Count].Data = binaryReader.ReadString();
                    Count++;
                }

                for (int i = 0; i < NodesByID.Count; i++)
                {
                    NodesByID[i].Random = NodesByID[LinkAtRandom.Dequeue()];

                    if (i == 0)
                    {
                        Head = NodesByID[i];
                        NodesByID[i].Next = NodesByID[i + 1];
                    }
                    else if (i == Count - 1)
                    {
                        Tail = NodesByID[i];
                        NodesByID[i].Previous = NodesByID[i - 1];
                    }
                    else
                    {
                        NodesByID[i].Previous = NodesByID[i - 1];
                        NodesByID[i].Next = NodesByID[i + 1];
                    }
                }
            }
        }
    }
    
}