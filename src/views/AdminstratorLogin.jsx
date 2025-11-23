import React from 'react'
import './styles/AdminstratorLogin.css'

const AdminstratorLogin = () =>{
    return(
        <div className='container'>
            <div className='header'>
                <div className='title_page'>ورود</div>
                <div className='underline'></div>
            </div>
            <div className='inputs'>
                <div className='input'>
                    <input type='text' placeholder='نام کاربری'/>
                </div>
                <div className='input'>
                    <input type='password' placeholder='رمز عبور'/>
                </div>
            </div>
            <div className='forgot_password'>فراموشی رمز عبور؟ کلیک کنید</div>
            <div className='Login'>ورود</div>
            <button>ورود</button>
        </div>
    )
}



export default AdminstratorLogin

