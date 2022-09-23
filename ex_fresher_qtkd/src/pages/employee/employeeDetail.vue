<template>
     <!-- Detail Employee  -->
     <div class="dialog__wrap"  >       
          <div class="dialog__infoEmp" >
            <div class="dialog__infoEmp--top">
                <div class="dialog__info">
                    <div class="dialog__info--name">
                        Thông tin nhân viên
                    </div>
                    <div class="dialog__info--checkbox">
                        <div class="checkbox__item">
                            <input type="checkbox" id="customer" class="checkbox__detail input__checkbox customer " 
                           
                            >
                            <label for="customer">Là khách hàng</label>
                        </div>
                        <div class="checkbox__item">
                            <input type="checkbox" id="supplier" class="checkbox__detail input__checkbox supplier" >
                            <label for="supplier">Là nhà cung cấp</label>
                        </div>
                    </div>
                </div>
                <div class="dialog__top--right">
                    <div class="dialog__question icon icon__question" ></div>
                    <div class="dialog__close icon icon__close"  @click="closeEmpDetail"></div>
                </div>
            </div>
            <div class="dialog__infoEmp--content">
                <div class="content__info">
                    <div class="content__info--left">
                        <div class="info__left--first">
                            <div class="info__item info__code">
                                <div class="lable-input">
                                    Mã
                                    <span class="color-red">*</span>
                                </div>
                                <div class="content__input tooltip" >
                                    <input type="text" class="input" maxlength="20"
                                    @keyup = 'validateEmptyCode'
                                    @focus = 'validateEmptyCode'
                                    :class="{'border-red' : emptyCode}"
                                    v-model="emp.employeeCode"
                                    :ref="'employeeCode'" >
                                    <div class="empty__input tooltiptext"  v-show="emptyCode" >Mã không được để trống</div>
                                </div>
                              
                               
                            </div>
                            <div class="info__item info__name">
                                <div class="lable-input">
                                    Tên
                                    <span class="color-red">*</span>
                                </div>
                                <div class="content__input tooltip">
                                    <input type="text" class="input" maxlength="100"
                                    @keyup = 'validateEmptyName'
                                    @focus = 'validateEmptyName'
                                    :class="{'border-red': emptyName}"
                                     v-model="emp.employeeName">
                                     <div class="empty__input tooltiptext"  v-show="emptyName">Tên không được để trống</div>
                                </div>
                               
                               
                            </div>
                        </div>
                       
                        <div class="info__item info__item--max">
                            <div class="lable-input">
                                Đơn vị
                                <span class="color-red">*</span>
                            </div>
                            <div class="content__input tooltip" >
                                <hCombobox  :url="'http://localhost:3000/department'" 
                                propValue="departmentID" propText="departmentName" 
                                v-model="emp.departmentID"
                                @getValue='getValue'
                               
                              
                                 />
                                 <!-- :class="{'border-red': emptyDepartment}" -->
                                 <div class="empty__input tooltiptext" v-show="emptyDepartment">Đơn vị không được để trống</div>
                           
                                
                                 <!-- <MCombobox :url="'http://localhost:3000/department'" 
                                :text="'departmentName'" 
                                @itemCombobox='itemCombobox'
                                :valueRender='dataCombobox'
                                :key = 'keyCombobox'/> -->
                            </div>
                           
                        </div>
                        <div class="info__item info__item--max">
                            <div class="lable-input">
                                Chức danh
                                
                            </div>
                            <div class="content__input">
                                <input class="input" type="text"  maxlength="255" v-model="emp.positionName">
                            </div>
                           
                        </div>
                      
                    </div>
                    <div class="content__info--right">
                        <div class="info__right--first">
                            <div class="info__item ">
                                <div class="lable-input">
                                   Ngày sinh
                                    
                                </div>
                                <div class="content__input">
                                    <input class="input input__date" type="date" v-model="emp.dateOfBirth" :max="maxDate" >
                                </div>
                               
                            </div>
                            <div class="info__item item__radio">
                                <div class="lable-input label-gender">
                                  Giới tính
                                    
                                </div>
                                <div class="content__radio">
                                    
                                    <div class="radio__item">
                                        <input type="radio" class="input__radio " id="radio__boy"  value="0" name="gender"                                           
                                         v-model="chooseGender"                                       
                                         >
                                        <label for="radio__boy">Nam</label>
                                    </div>
                                    <div class="radio__item">
                                        <input type="radio" class="input__radio " id="radio__girl" value="1" name="gender"                                         
                                        v-model="chooseGender"                                      
                                        >
                                        <label for="radio__girl">Nữ</label>
                                    </div>
                                    <div class="radio__item">
                                        <input type="radio" class="input__radio "  id="radio__other" value="2" name="gender"                                             
                                        v-model="chooseGender"                                      
                                        >
                                        <label for="radio__other">Khác</label>
                                    </div>
                                </div>                               
                            </div>
                        </div>
                        <div class="info__right--CMND">
                            <div class="info__item item__CMND">
                                <div class="lable-input">
                                 Số CMND                                    
                                </div>
                                <div class="content__input tooltip">
                                    <input class="input" 
                                     maxlength="25" type="text" 
                                     v-model="emp.identityNumber"
                                     @input="validateIdentity"
                                     :class="{'border-red': isIdentity}"
                                     
                                      >
                                      <div class="empty__input tooltiptext"  v-show="isIdentity">Số CMND không đúng</div>   
                                </div>  
                                                         
                            </div>
                            <div class="info__item ">
                                <div class="lable-input">
                                  Ngày cấp                                    
                                </div>
                                <div class="content__input">
                                    <input class="input input__date" type="date" v-model="emp.identityIssuedDate" :max="maxDate" >
                                </div>                             
                            </div>
                        </div>
                        <div class="info__item info__item--max">
                            <div class="lable-input">
                                Nơi cấp                                
                            </div>
                            <div class="content__input">
                                <input class="input" type="text"  maxlength="255" v-model="emp.identityIssuedPlace" >
                            </div>                           
                        </div>                       
                    </div>
                </div>
                <div class="content__address">
                    <div class="content__address--first">
                        <div class="info__item address__item--max">
                            <div class="lable-input">
                               Địa chỉ                                
                            </div>
                            <div class="content__input">
                                <input type="text" class="input"  maxlength="255" v-model="emp.address">
                            </div>
                           
                        </div>
                    </div>
                    <div class="info__other">
                        <div class="info__item item__other">
                            <div class="lable-input">
                               ĐT di động                             
                             </div>
                             <div class="content__input tooltip">
                                 <input type="text" class="input"  maxlength="50" 
                                 @input="validatePhoneNumber"
                                 v-model="emp.phoneNumbermobile"
                                 :class="{'border-red': isPhone}"
                                 >
                                 <div class="empty__input tooltiptext"  v-show="isPhone">Số điện thoại không đúng</div>  
                             </div>
                                 
                        </div>
                        <div class=" info__item item__other">
                            <div class="lable-input">
                                ĐT cố định                               
                             </div>
                             <div class="content__input tooltip">
                                 <input type="text" class="input"  maxlength="50" 
                                 @input="validatePhoneland"
                                 v-model="emp.phoneNumberlandline"
                                 :class="{'border-red': isPhoneLand}"
                                 >
                                 <div class="empty__input tooltiptext"  v-show="isPhoneLand">Số điện thoại không đúng</div>  
                             </div>
                             
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Email                                
                             </div>
                             <div class="content__input tooltip">
                                 <input type="text" class="input"  
                                 :class="{'border-red': errorEmail}"  
                                 maxlength="100" 
                                 @input="validateEmail"
                                 v-model="emp.email">
                                 <div class="empty__input tooltip"  v-show="errorEmail">Email chưa đúng định dạng</div>
                            </div>
                        </div>
                    </div>
                    <div class="info__other">
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Tài khoản ngân hàng                         
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input"  maxlength="25" v-model="emp.accountBank">
                             </div>
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Tên ngân hàng                                
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input"  maxlength="255" v-model="emp.nameBank">
                             </div>
                        </div>
                        <div class="info__item item__other">
                            <div class="lable-input">
                                Chi nhánh                                
                             </div>
                             <div class="content__input">
                                 <input type="text" class="input"  maxlength="255" v-model="emp.branchBank">
                             </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="dialog__infoEmp--bottom">
                <div class="dialog__left">
                    <div class="btn btn__cancel" @click="closeEmpDetail" >
                        Hủy
                    </div>
                </div>
                <div class="dialog__right">
                    <div class="btn btn__save">
                        Cất
                    </div>
                    <div class="btn btn__saveAdd"  @click="btnSaveOnClick" >Cất và thêm</div>
                </div>
            </div>
        </div> 
    </div>
       <!-- End detail Employee -->
</template>
<script>
    
    import MCombobox from '@/components/base/MCombobox.vue';
    import eNum from '@/js/common/eNum';
    import common from '@/js/common/common';


    /**
     * URL api 
     */
    var URL =process.env.VUE_APP_URL;

     /**
     * Hàm validate
     * author: HMHieu(14/9/2022)
     * @param {*} form: con trỏ vue
     * @param {object} objectEmpty {emptyCode, emptyName, emptyDepartment} 

     * 
     */
     function validate(form, objectEmpty){
        for(let key in objectEmpty){
            if(objectEmpty[key]){                             
                form.$emit('errorEmpty', key);              
                return false;
            }
        }
       
        return true;

       
        
       
   }

export default {
    
    component:{MCombobox},
    name:"employeeDetail",
    
    props:{
        closeDialog: Function,
        employeeSelected:Function,
        formMode:Number,
    },
    created(){       
        //gán data truyền từ cha sang con
        this.emp = this.employeeSelected;   

         //xử lý dữ liệu radio
         if(!common.checkEmptyData(this.emp.gender))
            this.chooseGender = this.emp.gender;          
    },
    mounted(){
        this.$refs['employeeCode'].focus();//focus vào item đầu tiên
        this.formatDate();//thay đổi dữ liệu ngày tháng theo format trước khi hiển thị 
    },   
    data() {
        return{
            showForm:true,     
            emp:{},
            maxDate:common.maxDate(),  
            chooseGender:Number,  
            dataCombobox:'',
            emptyCode: false,
            emptyName: false,
            emptyDepartment: true,
            errorEmail:false,
            isPhone:false,  
            isPhoneLand:false,  
            isIdentity:false,  
        }
    },
    
    methods:{
          /**
         * Đóng form dialog                                                         
         * Author: HMH(16/09/22)
        */
        closeEmpDetail(){
            this.closeDialog();
        },
        /**
         * format ngày tháng theo định dạng mong muốn
         * Author: HMH(19/09/22)
         */
        formatDate(){
            if(this.formMode===eNum.formMode.Edit){
                this.emp.dateOfBirth=common.formatDate(this.emp.dateOfBirth);
                this.emp.identityIssuedDate=common.formatDate(this.emp.identityIssuedDate);
            }
        },
         /**
         * Check giá trị input rỗng khi focus vào input mã nhân viên
         * author: HMHieu(21/09/2022)
         *
         */
         validateEmptyCode(){
            if(common.checkEmptyData(this.emp.employeeCode)){
            this.emptyCode = true;
            }else{
                 this.emptyCode = false;
            }
          
        },   
        /**
         * Check giá trị email có đúng định dạng chưa khi nhập dữ liệu vào input
         * author: HMHieu(21/09/2022)
         *
         */     
        validateEmail(){
            if(common.checkEmail(this.emp.email)){
            this.errorEmail = true;
            }else{
                 this.errorEmail = false;
            }          
        },  
        /**
         * Check giá trị input có đúng định dạng number chưa 
         * author: HMHieu(21/09/2022)
         *
         */     
         validatePhoneNumber(){
            if(common.checkNumber(this.emp.phoneNumbermobile)){
            this.isPhone = true;
            }else{
                 this.isPhone = false;
            }      
            
            
        },
        validatePhoneland(){
            if(common.checkNumber(this.emp.phoneNumberlandline)){
                this.isPhoneLand = true;
            }else{
                 this.isPhoneLand = false;
            }      
        },
        validateIdentity(){
            if(common.checkNumber(this.emp.identityNumber)){
            this.isIdentity = true;
            }else{
                 this.isIdentity = false;
            }         
        },
      
        /**
         * Check giá trị input rỗng khi focus vào input tên nhân viên
         *  author: HMHieu(21/09/2022)
         *
         */
        validateEmptyName(){
            if(common.checkEmptyData(this.emp.employeeName)){
                this.emptyName = true;
            }else {
                this.emptyName = false;
            }
        },
        getValue(items){       
            this.emp.departmentID = items.departmentID;
            this.emp.DepartmentName = items.DepartmentName;
            this.emp.DepartmentCode = items.DepartmentCode;
            this.empDepartmentName = this.emp.DepartmentName;   
            if(common.checkEmptyData(this.emp.departmentID)){
                this.emptyDepartment = true;
            }else  this.emptyDepartment = false;
            console.log('log',this.emp.departmentID)
        },

      
          
        
         /**
         * Thêm mới hoặc sửa dữ liệu theo formMode
         * Author: HMH(19/09/22)
         */
       
        btnSaveOnClick() {
            var me = this; 
            console.log(this.emp.departmentName)
              // validate dữ liệu:
            let checkEmpty = {
                EmptyCode : this.emptyCode, EmptyName : this.emptyName//, EmptyDepartment : this.emptyDepartment
            } 
            
           
            let valid = validate(me, checkEmpty);
            if(valid){
                if(me.formMode===eNum.formMode.ADD){
                var method = "POST";
                fetch(URL, {                
                method: method,
                headers: {
                "Content-Type": "application/json",
                },
                body: JSON.stringify(me.emp),
            })
                .then((res) => res.json())
                .then((res) => {
            
                    me.$emit("closeDialog");    
                    console.log(res);
                })
                .catch((res) => {
                console.log(res);
                
                });
            }              
          
            // Cất dữ liệu:

            if (me.formMode === eNum.formMode.Edit) {
                method = "PUT";
                URL = URL + `/${me.emp.id}`;
                fetch(URL, {            
                    method: method,
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(me.emp),})
                    .then((res) => res.json())
                    .then((res) => {
                
                        me.$emit("closeDialog");    
                        console.log(res);
                    })
                    .catch((res) => {
                        console.log(res);                    
                    });
            }  
            }         
           
        },            
    },
}
</script>