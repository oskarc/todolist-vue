<template>
    <div>
        <h1>Skapa användare</h1>
        <form @submit.prevent="handleSubmit">
            <div class="form-group">
                <label for="username">Förnamn</label>
                <input type="text" v-model="FirstName" name="username" class="form-control" :class="{ 'is-invalid': submitted && !FirstName }" />
                <div v-show="submitted && !FirstName" class="invalid-feedback">First name is required</div>
            </div>
            <div class="form-group">
                <label for="username">Efternamn</label>
                <input type="text" v-model="LastName" name="username" class="form-control" :class="{ 'is-invalid': submitted && !LastName }" />
                <div v-show="submitted && !LastName" class="invalid-feedback">Last name is required</div>
            </div>
            <div class="form-group">
                <label for="username">Användarnamn</label>
                <input type="text" v-model="UserName" name="username" class="form-control" :class="{ 'is-invalid': submitted && !UserName }" />
                <div v-show="submitted && !UserName" class="invalid-feedback">Username is required</div>
            </div>
            <div class="form-group">
                <label htmlFor="password">Lösenord</label>
                <input type="password" v-model="Password" name="password" class="form-control" :class="{ 'is-invalid': submitted && !Password }" />
                <div v-show="submitted && !Password" class="invalid-feedback">Password is required</div>
            </div>
            <div class="form-group">
                <label htmlFor="password">Återge Lösenord</label>
                <input type="password" v-model="reEnterPassword" name="password" class="form-control" :class="{ 'is-invalid': submitted && !reEnterPassword }" />
                <div v-show="submitted && !reEnterPassword" class="invalid-feedback">Reenter password required</div>
            </div>
            <div class="form-group">
                <button class="btn btn-primary" :disabled="loading">Registrera</button>
                <img v-show="loading" src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
            </div>
            <div v-if="error" class="alert alert-danger">{{error}}</div>
        </form>
    </div>
</template>

<script>
import { router } from '@/router';
import { userService } from '@/_services/user.service';

export default {
    data () {
        return {
            FirstName: "Förnamn",
            LastName: "Efternamn",
            UserName: "",
            Password: "Admin123",
            reEnterPassword: "Admin123",
            submitted: false,
            loading: false,
            returnUrl: '/login',
            error: ''
        }
    },
    methods: {
        handleSubmit (e) {
            console.log("hit!")
            this.submitted = true;
            const { UserName, Password, FirstName, LastName } = this;
            

            // stop here if form is invalid
            if (!(UserName && Password && FirstName && LastName)) {
                console.log("FirstName:" + FirstName +  
                            "LastName:" + LastName +  
                            "UserName:" + UserName + 
                            "Password:" + Password)
                return;
            }

            this.loading = true;
            userService.register(FirstName, LastName, UserName, Password)
                .then(
                    user => router.push(this.returnUrl),
                    error => {
                        this.error = error;
                        this.loading = false;
                    },
                    this.resetForms()
                );
        },
        resetForms() {
            this.FirstName = "Förnamn";
            this.LastName = "Efternamn";
            this.UserName = "";
            this.Password = "Admin123";
            this.reEnterPassword = "Admin123";
            this.submitted = false;
            this.loading = false;
            this.returnUrl = '';
            this.error = '';
        }
    }
};
</script>
<style scoped>

input {
    margin: 5px;
    display: block;
    margin-left: 42%;
}

label {
    display: block;
}

</style>
