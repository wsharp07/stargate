<template>
  <div class="notification">
    <form enctype="multipart/form-data">
      <div class="file has-name is-fullwidth is-info">
        <label class="file-label">
          <input class="file-input"
            id="fileUpload"
            name="fileUpload"
            type="file" 
            :disabled="isSaving"
            @change="fileUploadChange"
            v-validate="'required'" />
          
          <span class="file-cta">
            <span class="file-icon">
              <i class="fa fa-folder"></i>
            </span>
            <span class="file-label">
              Choose a file…
            </span>
          </span>
          <span class="file-name">
            <i>{{ fileName }}</i>
          </span>
          
        </label>
        <a class="button" 
          @click="uploadClick">
          <span class="file-icon">
            <i class="fa fa-upload"></i>
          </span>
          Upload File
        </a>
      </div>
      <!-- Render Errors -->
      <span v-show="errors.has('fileUpload')" class="help is-danger is-size-6 has-text-weight-semibold">
        {{ errors.first('fileUpload') }}
      </span>
    </form>
  </div>
</template>

<script>
import {FileApi} from '../services/file-upload.service';
import Router from '../router';

export default {
  name: "file-upload",
  data() {
    return {
      uploadError: null,
      fileToUpload: null,
      fileName: "Select a file to upload"
    };
  },
  methods: {
    reset() {
      // reset form to initial state
      this.fileToUpload = null;
      this.uploadError = null;
      this.fileName = "Select a file to upload";
    },
    save(formData) {
      let self = this;
      FileApi.upload(formData)
        .then(function(response) {
          Router.push({name: 'File View', params: { id: response.data.id }});
        })
        .catch(function(error) {
          console.log(error);
        });
    },
    fileUploadChange(e) {
      this.fileToUpload = e.target.files[0];
      this.fileName = this.fileToUpload.name;
    },
    uploadClick(e) {
      this.$validator.validateAll();

      const formData = new FormData();
      formData.append("file", this.fileToUpload, this.fileToUpload.name);

      this.save(formData);
    }
  },
  created() {
    this.reset();
  }
};
</script>