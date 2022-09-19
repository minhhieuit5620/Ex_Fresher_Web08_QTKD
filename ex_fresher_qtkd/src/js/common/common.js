
export default {
    formatEmptyDate(){

    },
    /**
     * Format date trước khi hiển thị lên màn hình
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
    formatDateTable(data) {
        try {
            if (data) {
           
                data = new Date(data);
                let day = data.getDate();
                if(day<10)day='0'+day;
                let month = data.getMonth() + 1;
                if(month<10)month='0'+month;
                let year = data.getFullYear();
                data=`${day}/${month}/${year}`;             
                return data;
            }
        } 
        catch (error) {
            console.log(error);
        }
       
      
    },
     /**
     * Xét khoảng được chọn trong date
     * Author: HMHieu(19/09/2022)
     *  */
    maxDate() {       
        try {        
            let date = new Date(),
            year = date.getFullYear().toString(),
            month = (date.getMonth() + 1).toString().padStart(2, '0'),
            day = date.getDate().toString().padStart(2, '0');
            return `${year}-${month}-${day}`;
            
          
        } catch (error) {
            console.log(error)
        }

    },

}
