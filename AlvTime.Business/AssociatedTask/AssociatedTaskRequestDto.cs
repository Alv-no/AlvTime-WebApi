﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AlvTime.Business.AssociatedTask
{
    public class AssociatedTaskRequestDto
    {
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public DateTime FromDate { get; set; }
    }
}
