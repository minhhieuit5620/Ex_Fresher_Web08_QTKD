
export default {
    formatEmptyDate(){

    },
    /**
     * Format date trước khi hiển thị lên formDialog màn hình
     * Author: HMHieu(19/09/2022)
     *  */

    formatDate(dateSrc) {
        try {
            if(dateSrc){
                let date = new Date(dateSrc);
                date = date.toISOString().substring(0, 10);
                return date;
            }      
        } catch (error) {
            console.log(error);
    
        }
    },
    /**
     * Format date trước khi hiển thị lên table
     * Author: HMHieu(19/09/2022)
     *  */
    formatDateTable(data) {
       
            if (data) {
           
                data = new Date(data);
                let day = data.getDate();
                if(day<10)
                {
                    day='0'+day;
                }
                let month = data.getMonth() + 1;
                if(month<10){
                    month='0'+month;
                }
                let year = data.getFullYear();
                data=`${day}/${month}/${year}`;             
                return data;
            }else{
                return "";
            }
      
       
      
    },
     /**
     * Xét khoảng được chọn trong date
     * Author: HMHieu(19/09/2022)
     *  */
    maxDate() {       
        try {        
            let date = new Date();
           let year = date.getFullYear().toString();
            if(year>date.getFullYear()||(year<1970)){
                year=date.getFullYear();
            }
           let month = (date.getMonth() + 1).toString().padStart(2, '0');
           let day = date.getDate().toString().padStart(2, '0');
            return `${year}-${month}-${day}`;                      
        } 
        catch (error) {
            console.log(error)
        }

    },
    /**
     * Check giá trị trong input có rỗng hay không
     * Author: HMHieu(21/09/2022)
     *  */
    checkEmptyData(data){
        if(data === '' || data == undefined || data == null){
            return true;
        }       
        else{
            data = data.toString().trim();
            if(data === '') return true;
            return false; 
        }
    },

    checkLengthCode(data){
        if(data.count>20){
            data=data.substring(0,19);
            return data;
        }
    },
      /**
         * Kiểm tra định dạng email
         */
    checkEmail(email){
        if(email != '' || email != undefined){           
            let regex = new RegExp('[a-z0-9]+@[a-z]+[a-z]{2,3}');
            if(!regex.test(email)){
                return true;
            }
            else{
                return false;                
            }
        }
     
    },
     /**
     * Kiểm tra định dạng số
     */
    checkNumber(data){
        if(data != '' || data != undefined){
            let regex=new RegExp('^[1-9]*$');
            if(!regex.test(data)){
                return true;
            }
            else{
                return false;
            }
            
        }
       
    }


}
