﻿select FirstName,Picture,Start_time,End_time,pay from tbl_worker,tbl_Duty,tbl_blocks,tbl_Farm,tbl_Attendance where tbl_worker.WorkersId = tbl_Duty.WorkerID AND tbl_Duty.RosterID = tbl_Attendance.RosterID AND tbl_Attendance.blockid=19 AND ;
