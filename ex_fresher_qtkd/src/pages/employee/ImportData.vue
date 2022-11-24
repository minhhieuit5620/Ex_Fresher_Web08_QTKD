<template>
    <div class="dialog__wrap" id="range">
        <div class="dialog__infoEmp dialog__import">
            <div class="dialog__import--top">
                <div class="dialog__info">
                    <div class="dialog__info--name ">
                        Nhập khẩu nhân viên
                    </div>

                </div>
                <div class="tooltip">
                    <div class="dialog__close icon icon__close--importESC close__import" @click="closeDialogImport()">
                    </div>
                    <span class="tooltiptext tooltipIcon tooltip__right">Đóng(ESC)</span>
                </div>

            </div>
            <div class="dialog__import--content">
                <div class="current__step dialog__info--name">
                    <div v-show="stepOne">Bước 1: Chọn tệp nguồn</div>
                    <div v-show="stepTwo">Bước 2: Kiểm tra dữ liệu</div>
                    <div v-show="stepThree">Bước 3: Kết quả nhập khẩu</div>
                </div>
                <div class="dashboard__import">
                    <div class="dashboard__import--left">
                        <button class="input step step1" :class="{ active__step: stepOne }" @click="chooseStepOne">1.
                            Chọn tệp nguồn</button>
                        <button class="input step step2"
                            :class="{ active__step: stepTwo, disable__class: this.nameFile.length === 0 }"
                            :disabled="this.nameFile.length === 0" @click="checkImport">2. Kiểm tra dữ liệu</button>
                        <button class="input step step3"
                            :class="{ active__step: stepThree, disable__class: this.dataCheckValue.length === 0 || !stepTwo || stepOne }"
                            :disabled="this.dataCheckValue.length === 0 || !stepTwo || stepOne" @click="importData">3. Kết quả
                            nhập khẩu</button>
                    </div>


                    <div class="dashboard__import--right">
                        <div class="Show__step Show__step1" v-show="stepOne">
                            <div class="Show__step--innner guide">Chọn dữ liệu nhân viên đã chuẩn bị để nhập vào phần
                                mềm</div>
                            <div class="Show__step--innner choose__file">
                                <input type="text" v-model="nameFile" class="input chosse__text" disabled>
                                <input type="file" @change="changeInput" ref="file" class="chosse__file--hide">
                                <button class="btn btn__import" @click="clickChosseFile">Chọn</button>
                            </div>
                            <div class="Show__step--innner template__excel">Chưa có tệp mẫu để chuẩn bị dữ liệu? Tải tệp
                                excel mẫu mà phần mềm cung cấp để chuẩn bị dữ liệu nhập khẩu <a
                                    href="Mau_danh_muc_nhan_vien.xlsx" download>Tại đây</a> </div>
                        </div>
                        <div class="Show__step Show__step2" v-show="stepTwo">
                            <div class="top__data--import" v-if="this.dataCheckValue.length > 0">
                                <div class="throw__status--data data__valid">


                                    {{ this.recordSuccess }} / {{ this.dataCheckValue.length }} dòng hợp lệ
                                </div>
                                <div class="throw__status--data data__invalid">
                                    {{ this.recordError }} / {{ this.dataCheckValue.length }} dòng không hợp lệ
                                </div>
                            </div>

                            <div class="content__data data__import">
                                <div class="empty__value--import" v-if="this.dataCheckValue.length === 0">File dữ liệu của
                                    bạn bị trống, vui lòng kiểm tra lại.</div>
                                <table class="table table-hover table__import" v-if="this.dataCheckValue.length > 0">
                                    <thead>
                                        <tr class="">
                                            <th class="table__border--left table__border--top">STT</th>
                                            <th class=" width__150 table__border--top">Mã nhân viên</th>
                                            <th class="width__200 table__border--top">Tên nhân viên</th>
                                            <th class="width__100 table__border--top">Giới tính</th>
                                            <th class="width__100 table__border--top">Ngày sinh</th>
                                            <th class="width__150 table__border--top">Tên đơn vị</th>
                                            <th class="width__100 table__border--top">
                                                <div class="tooltip">Số CMND <span class="tooltiptext tooltipAronym">Số
                                                        chứng minh nhân dân</span></div>
                                            </th>
                                            <th class="width__150 table__border--top">Ngày cấp</th>
                                            <th class="width__150 table__border--top">Chức danh</th>
                                            <th
                                                class="width__400 table__border--top column__sticky column__edit column__edit--th">
                                                Tình trạng</th>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for=" (item, index) in dataCheckValue" :key="item.data.employeeCode"
                                            :class="{ chooseElement: chooseEl[index] }" @click="chooseTr(item, index)">
                                            <td class="table__border--left">{{ index + 1 }}</td>
                                            <td class="">{{ item.data.employeeCode }}</td>
                                            <td class="">{{ item.data.employeeName }}</td>
                                            <td class="">
                                                <span v-if="item.data.gender === this.male">Nam </span>
                                                <span v-if="item.data.gender === this.feMale">Nữ</span>
                                                <span v-if="item.data.gender === this.other">Khác</span>

                                            </td>
                                            <td class="">
                                                {{ formatDateTable(item.data.dateOfBirth) }}
                                            </td>
                                            <td class="">{{ item.data.departmentName }}</td>
                                            <td class="">
                                                {{ item.data.identityNumber }}
                                            </td>
                                            <td class="">
                                                {{ formatDateTable(item.data.identityIssuedDate) }}
                                            </td>
                                            <td class="">{{ item.data.positionName }}</td>
                                            <td class="column__sticky   column__error--import edit__td column__edit"
                                                :class="{ chooseElement: chooseEl[index] }">

                                                <div class="errorImport" v-if="item.stringError.length > 0">
                                                    <div  v-for=" (itemError, indexError) in item.stringError"
                                                        :key="indexError">                                                       
                                                        {{ itemError }}
                                                    </div>

                                                </div>
                                                <div v-if="item.stringError.length === 0">
                                                    Hợp lệ
                                                </div>
                                            </td>

                                        </tr>

                                    </tbody>
                                </table>

                            </div>
                            <div class="download__invalid">Tải về tập tin chứa các dòng nhập khẩu không thành công <a
                                    href="#">Tại đây.</a></div>
                        </div>
                        <div class="Show__step Show__step3" v-show="stepThree">
                            <div class="title__result">Kết quả nhập khẩu</div>
                            <div class="Show__step--innner">Tải về tập tin chứa kết quả nhập khẩu <span
                                    class="export__Import" @click="exportImport">Tại đây.</span></div>
                            <div class="Show__step--innner result__import">+ Số dòng nhập khẩu thành công:
                                {{ this.importSuccess }}</div>
                            <div class="Show__step--innner result__import">+ Số dòng nhập khẩu không thành công:
                                {{ this.importError }}</div>

                        </div>
                    </div>

                </div>
            </div>

            <div class="dialog__import--bottom">
                <button class="btn btn__import--left btn__import ">
                    <div class="icon icon__ask--import"></div>
                    <div>Giúp</div>

                </button>
                <div class=" btn__import--right">
                    <button class="btn__import--back btn btn__import " v-show="!stepThree" :disabled="stepOne"
                        :class="{ disable__class: stepOne }" @click="chooseStepOne">
                        <div class="icon icon__back"></div>

                        <div>Quay lại</div>

                    </button>
                    <button class="btn__import--action btn btn__import" v-show="stepOne"
                        :class="{ disable__class: nameFile.length === 0 || nameFile === null || nameFile === '' || nameFile === undefined }"
                        :disabled="nameFile.length === 0 || nameFile === null || nameFile === '' || nameFile === undefined"
                        @click="checkImport">
                        <div class="icon icon__continue--import"></div>

                        <div>Tiếp tục</div>

                    </button>
                    <button class="btn__import--action btn btn__import" v-show="stepTwo"
                        :class="{ disable__class: this.dataCheckValue.length === 0 }"
                        :disabled="this.dataCheckValue.length === 0" @click="importData">
                        <div class="icon icon__action--import"></div>

                        <div>Thực hiện</div>

                    </button>
                    <button class="btn__import--cancel btn btn__import" v-show="!stepThree"
                        @click="closeDialogIntoImport()">

                        <div class="icon icon__cancel--import"></div>

                        <div>Hủy bỏ</div>
                    </button>
                    <button class="btn__import--cancel btn btn__import" v-show="stepThree" @click="closeDialogImport()">

                        <div class="icon icon__close--import"></div>

                        <div>Đóng</div>
                    </button>

                </div>
            </div>
        </div>
    </div>
</template>

<script>
/* eslint-disable */
const loadDataURL = process.env.VUE_APP_URL;

import common from '@/js/common/common';
import Resource from '@/js/common/resource';
import eNum from '@/js/common/eNum';
import baseApi from '@/js/common/baseApi';
import urlAPI from "@/js/common/urlApi"
import urlApi from '@/js/common/urlApi';



export default {
    data() {
        return {
            stepOne: true,
            stepTwo: false,
            stepThree: false,
            stepNextTwo: false,
            stepNextThree: false,
            nameFile: "",
            checkFileEmpty: false,
            checktype: false,
            dataCheckValue: [],

            chooseEl: [],

            dataChooseTr: [],
            dataSuccess: [],

            recordError: Number,
            recordSuccess: Number,

            importSuccess: Number,
            importError: Number,
            //gender
            male: eNum.gender.Male,
            feMale: eNum.gender.Femail,
            other: eNum.gender.Other,

            fileNow: ""


        }
    },
    props: {
        closeImportEx: Function,
        loadData: Function
    },
    mounted() {
        document.addEventListener("keydown", this.escClose);

    },
    // unmounted(){
    //     document.removeEventListener("keydown",escClose)
    // },

    methods: {

        /**
         * Đóng form
         */
        closeDialogImport() {
            this.closeImportEx();
        },
        /********************
        * Đóng form khi đang thực hiện bước 2
        * HMHieu 15-11-2022
        * 
        */
        closeDialogIntoImport() {
            if (this.dataSuccess.length > 0) {
                this.$emit(Resource.emit.showPopupClose);
            } else {
                this.closeDialogImport()
            }
        },
        /**
         * mở sheet chọn file
         */
        chooseStepOne() {
            this.stepOne = true,
            this.stepTwo = false,
            this.stepThree = false,
            this.nameFile = '';        

        },

        /**
        * mở sheet kiểm tra dữ liệu file
        */
        chooseStepTwo() {
            this.stepOne = false,
            this.stepTwo = true,
            this.stepThree = false,
            this.stepNextTwo = true
        },

        /**
        * mở sheet xem kết quả
        */
        chooseStepThree() {
            this.stepOne = false,
            this.stepTwo = false,
            this.stepThree = true
            this.stepNextThree = true
        },
        /**
         * xử lí nút chọn file
         */
        clickChosseFile() {
            this.nameFile = '';
            this.$refs.file.value = null;
            this.$refs.file.click();

            //this.nameFile=this.$refs.file.files[0].name;   
        },
        /**
         * khi đã chọn thì gán tên file vào ô input
         */
        changeInput() {
            this.nameFile = this.$refs.file.files[0].name;
            //this.fileNow = this.$refs.file.files[0]; 
            // this.$refs.file.reset()           
        },

        /********************
        * kiểm tra dữ liệu của fiel đầu vào
        * HMHieu 14-11-2022
        * 
        */
        async checkImport() {

            try {
                this.checkEmptyFileChoose();
                if (this.checktype === false) {  //validate thành công                                                                        
                    let file = this.$refs.file.files[0];
                    var fileImport = new FormData();
                    fileImport.append('file', file)
                    let method = Resource.method.POST;
                    debugger;
                    try {
                        var res = await baseApi.fetchAPIFile(urlAPI.importData.checkImport, fileImport, method);
                    
                    let status = res.status;
                    if (res.ok) {
                        const responseJSON = await res.json();
                        this.dataCheckValue = responseJSON.data;
                        // console.log(this.dataCheckValue.data);
                        // debugger
                        this.recordSuccess = 0;
                        for (let i = 0; i < this.dataCheckValue.length; i++) {
                            if (this.dataCheckValue[i].stringError.length === 0) {
                                this.recordSuccess++;
                                this.dataSuccess.push(this.dataCheckValue[i].data.employeeCode);
                            }
                        }
                        this.recordError = this.dataCheckValue.length - this.recordSuccess;
                        //thành công thì chuyển sang bước tiếp
                        this.chooseStepTwo();
                    }
                    else {

                        if (status === eNum.errorBackEnd.User) {
                            const responseJSON = await res.json();
                            let errorCode = responseJSON.errorCode;  //Mã lỗi trả về được quy định ở BE                           
                            if (errorCode === eNum.errorBackEnd.notFileImport) {
                                this.$emit(Resource.emit.errorBackEnd, errorCode);
                            }
                            else {
                                this.$emit(Resource.emit.errorBackEnd, errorCode);
                            }
                        } 
                        else {
                            this.$emit(Resource.emit.errorBackEnd, eNum.errorBackEnd.Ser);
                        }
                    }}
                    catch (error) {
                        this.$emit(Resource.emit.errorBackEnd, eNum.errorBackEnd.Ser);
                        console.log(error)
                    }
                }               
            } catch (error) {
                this.$emit(Resource.emit.errorBackEnd, eNum.errorFE.UserActionError);
                console.log(error);
            }

        },

        /********************
        * Chọn hàng dữ liệu để thực hiện nhập khẩu
        * và thay đổi giao diện
        * HMHieu 14-11-2022
        * 
        */
        chooseTr(item, index) {
            //kiểm tra xem dòng dữ liệu có chứa lỗi hay không
            var check = item.stringError.length;
            if (check > 0) {//nếu dòng có thì không cho chọn
                this.chooseEl[index] = false;
            } else {//nếu dòng không có thì được chọn

                this.chooseEl[index] = !this.chooseEl[index];

                if (this.chooseEl[index] === true) {//chọn dữ liệu để lưu vào mảng
                    if (!this.dataChooseTr.includes(this.dataCheckValue[index].data)) {
                        this.dataChooseTr.push(this.dataCheckValue[index].data.employeeCode);
                    }
                }
                else {
                    //xóa dữ liệu khỏi mảng
                    this.dataChooseTr.splice(this.dataChooseTr.indexOf(this.dataCheckValue[index].data.employeeCode), 1);
                }
            }
        },
        /********************
        * Thực hiện nhập khẩu dữ liệu 
        * HMHieu 14-11-2022
        * 
        */
        async importData() {
            try {
                var method = Resource.method.POST;
                var dataImport = [];
                if (this.dataChooseTr.length > 0) {//kiểm tra xem có đối tượng được chọn không
                    dataImport = this.dataChooseTr;
                } else {
                    dataImport = this.dataSuccess;
                }
                try {
                    var res = await baseApi.fetchAPIBodyV2(dataImport, urlAPI.importData.import, method);
                    if (dataImport.length === 0 || dataImport === null) {
                        this.$emit(Resource.emit.noRecordValid)
                    }
                    else {
                        let status = res.status;
                        if (res.ok) {
                            const responseJSON = await res.json();
                            this.importSuccess = responseJSON.data;
                            this.importError = this.dataCheckValue.length - this.importSuccess;
                            this.chooseStepThree();
                            this.dataChooseTr = [];
                            this.dataSuccess = [];
                            this.chooseEl=[];
                        }
                        else {
                            if (status === eNum.errorBackEnd.User) {
                                const responseJSON = await res.json();
                                let errorCode = responseJSON.errorCode;  //Mã lỗi trả về được quy định ở BE                                                 
                                if (errorCode === eNum.errorBackEnd.notFileImport) {
                                    this.$emit(Resource.emit.errorBackEnd, errorCode);
                                } else {
                                    this.$emit(Resource.emit.errorBackEnd, errorCode);
                                }
                            }
                            else {
                                this.$emit(Resource.emit.errorBackEnd, eNum.errorBackEnd.Ser);
                            }
                        }
                    }
                } catch (error) {
                    this.$emit(Resource.emit.errorBackEnd, eNum.errorBackEnd.Ser);
                    console.log(error);
                }
               
            } catch (error) {
                this.$emit(Resource.emit.errorBackEnd, eNum.errorFE.UserActionError);
                console.log(error)
            }
        },

        /********************
        * xuất khẩu dữ liệu import thành công
        * HMHieu 17-11-2022
        * 
        */
        exportImport() {
            let method = Resource.method.GET;
            var token = localStorage.getItem(Resource.Manager.token.accessToken);
            try {
                let check = common.checkEmptyData(token);
                if (check) {
                    this.$router.push('/login');
                } else {
                    var URL = urlApi.importData.exportImportSuccess;
                    // window.location.href=changeURL;                                                                                   
                    fetch(URL, {
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `Bearer ${token}`
                        }
                    })
                        .then((res) => { return res.blob(); })
                        .then((data) => {
                            var a = document.createElement("a");
                            a.href = window.URL.createObjectURL(data);
                            a.download = Resource.NameFileDownLoad.NameFile;
                            a.click();
                        }).catch((res) => {
                            this.$emit(Resource.emit.errorBackEnd, eNum.errorBackEnd.Ser);
                            console.log(res);
                        });
                }
            } catch (error) {
                this.$emit(Resource.emit.errorBackEnd, eNum.errorBackEnd.Ser);
                console.log(error);
            }
        },

        /********************
        * đóng form bằng keycode ESC
        * HMHieu 17-11-2022
        * 
        */
        escClose(event) {
            let key = event.keyCode;
            if (key === eNum.KeyCode.ESC) {
                this.closeDialogIntoImport()
            }

        },

        /**
         * kiểm  tra file đầu vào đã có hay chưa
         * nếu có thì kiểm tra tiếp loại file đã đúng hay chưa      
         */
        checkEmptyFileChoose() {
            //kiểm tra file đầu vào tồn tại hay chưa
            if (common.checkEmptyData(this.nameFile)) {
                this.checkFileEmpty = true;
                //this.$emit('errorEmptyFile');
            } else {
                //kiểm tra file đầu vào có đúng loại file hay không
                if (!common.checkIsExcel(this.nameFile)) {
                    this.checktype = true;
                    this.$emit(Resource.emit.errorTypeFile);

                } else {
                    this.checktype = false;
                }

            }
        },

        /**
        * format date hiển thị list danh sách nhân viên nhập khẩu
        * Author: HMHieu(14/11/22)
        */
        formatDateTable(date) {
            let dateFormat = common.formatDateTable(date);
            return dateFormat;
        },

    }

}
</script>

<style>
.dialog__import {
    min-width: 1037px;

    font-size: 14px;
    border: none;
    border: 8px solid #005995;
}

.btn__import {
    background-color: #fff;
    color: #000;
    border: 1px solid #ccc;

}

.dialog__import--top {
    display: flex;
    justify-content: space-between;
    /* padding: 4px; */
    padding-right: 5px;
    padding-left: 12px;
    background-color: #005995;
    color: #fff;
    /* border-radius: 4px 4px 0px 0px; */
}

.dialog__import--top .dialog__info--name{
    font-size: 14px;    
    line-height: 32px;
}

.current__step {
    /* padding-left:12px;
    padding-bottom: 8px; */
    font-size: 14px;
    padding: 8px;
    padding-left: 12px;
    font-weight: bold;
}

.dialog__import--content {
    display: flex;
    flex-direction: column;
}

.dialog__import--bottom {
    display: flex;
    justify-content: space-between;
    padding: 12px;
    border-radius: 0px 0px 4px 4px;
}

.dashboard__import {
    display: flex;

}

.dashboard__import--left {
    padding-left: 12px;
    /* padding-right: 26px; */
}

.dashboard__import--right {
    border: 1px solid #ccc;
    min-height: 500px;
    min-width: 805px;
    margin-right: 10px;
}

.btn__import--right {
    display: flex;
}

.btn__import--back {
    margin-right: 10px;
    padding-left: 8px;
    display: flex;
}

.active__step {
    background-color: #add2ed;
}

.btn__import--action {
    margin-right: 10px;
    padding-left: 8px;
    display: flex;
}

.btn__import--cancel {
    padding-left: 8px;
    display: flex;
}

.Show__step {
    padding: 12px;
}

.Show__step--innner {
    padding-bottom: 12px;
}

.step {
    font-weight: bold;
    text-align: left;
    height: 32px;
    width: 180px;
    line-height: 32px;
    margin-bottom: 8px;
}

.chosse__file--hide {
    display: none;
}

.chosse__text {
    width: 500px;
    margin-right: 16px;
}

.top__data--import {
    display: flex;
    margin-left: 10px;
}

.throw__status--data {
    padding-right: 200px;
    font-weight: bold;
}

.data__import {
    width: 760px;
    height: 419px;
}

.download__invalid {

    padding-top: 10px;
    padding-left: 10px;

}

.table__import {
    width: 800px;

}

.table__import th {
    background-color: #f1f1f1;
}

.table__import tr:hover td {
    background-color: #bedbf0;
}

.title__result {
    font-size: 32px;
    font-weight: bold;
    padding-bottom: 12px;
}

.table__border--left {
    border-left: 1px solid #babec5;
}

.table__border--top {
    border-top: 1px solid #babec5;

}

/* .input:hover {
    cursor: pointer;
} */

.disable__class {
    cursor: default !important;
    opacity: 0.7;
}

.errorImport {
    color: #e51616;
}

.table__import tr:active td {
    background-color: #bedbf0;
}

.chooseElement {
    background-color: #bedbf0;
}

.empty__value--import {
    padding: 12px;
    font-size: 16px;
    font-weight: bold;
}

.btn__import--left {
    display: flex;
    padding-left: 8px;
}

.close__import {
    color: #fff;
}

.column__error--import {
    justify-content: left;
    height: unset;
}

.export__Import {
    color: rgb(12, 12, 244);
    cursor: pointer;
    text-decoration: underline;
}
.errorString{

    height: 20px;
}

/* .export__Import:hover{
    text-decoration: underline;

} */
</style>