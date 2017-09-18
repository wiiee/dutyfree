namespace DutyFree.Service.Model
{
    using System.Collections.Generic;

    public class LinkedList
    {
        public string Id { get; set; }
        public List<LinkedList> Nexts { get; set; }
    }
}
