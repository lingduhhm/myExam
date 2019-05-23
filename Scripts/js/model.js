function Assignset() {
    this.exeindex = 0; //记录当前task得索引
    this.exelist = []; //保存task的数组
    this.sutassign = [];//保存sut的数组
    this.clear = function clear() {
        this.exeindex = 0;
        this.exelist = [];
        for (var i = 0; i < sutassign.length; i++) {
            this.sutassign[i].exeassign = []
        }
    }

}
//data struct for task reading(soon to obselete)
function sutassign(name, index) {
    this.SUT_id = index;
    this.SUTid = name;
    this.exeassign = [];
}


//data struct for task assigning(soon to obselete)
function Task() {
    this.testItem = 0;
    this.name = "";
    this.loopMode = "";
    this.loopCount = 0;
    this.ResumeTime = 0;
    this.ElapseTime = 0;
    this.S3ResumeTime = 0;
    this.S3ElapseTime = 0;
    this.S4ResumeTime = 0;
    this.S4ElapseTime = 0;
    this.restartElapseTime = 0;
}

//data struct for task assigning(soon to obselete)
function FeatureTask() {
    this.tcname = "";
    this.filename = "";
    this.count = 0;
    this.parameter = "";
    this.options = "";  
}