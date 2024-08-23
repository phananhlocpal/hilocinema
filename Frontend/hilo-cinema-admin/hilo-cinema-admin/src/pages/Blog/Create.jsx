import { useState, useEffect } from "react";
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalCloseButton, ModalBody, ModalFooter, Button, Select , Input} from "@chakra-ui/react";
import axios from "axios";
import PropTypes from "prop-types";
import { toBase64 } from "../../utils/toBase64";

const CreateBlogModal = ({ isOpen, onClose, fetchBlogs }) => {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [content, setContent] = useState('');
    const [type, setType] = useState('')
    const [img, setImage] = useState('')
    const employee = localStorage.getItem("account");

    console.log(employee)

    useEffect(() => {
        // Initialize CKEditor when component is mounted
        if (isOpen) {
            const script = document.createElement("script");
            script.src = "https://cdn.ckeditor.com/4.16.0/standard/ckeditor.js";
            script.async = true;
            script.onload = () => {
                if (window.CKEDITOR) {
                    window.CKEDITOR.replace('content');
                }
            };
            document.body.appendChild(script);
        }
    }, [isOpen]);

    const onSave = async () => {
        const token = localStorage.getItem("jwtToken");
        const currentDate = new Date().toISOString();

        const data = {
            title,
            description,
            content: window.CKEDITOR.instances.content.getData(),
            createdDate: currentDate,
            employeeId: employee.id,
            img: await toBase64(img), 
            status: "Active",
        };

        try {
            await axios.post("http://localhost:8000/BlogService", data, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                    Site: 'admin',
                    Role: 'admin'
                },
            });
            onClose(); 
            fetchBlogs();
        } catch (error) {
            console.error("Error saving blog:", error.response ? error.response.data : error.message);
        }
    };

    const handleImageChange = (event) => {
        setImage(event.target.files[0]);
    };

    return (
        <Modal isOpen={isOpen} onClose={onClose} size={"3xl"}>
            <ModalOverlay />
            <ModalContent>
                <ModalHeader>Create New Blog Post</ModalHeader>
                <ModalCloseButton />
                <ModalBody pb={2}>
                    <div className="py-6">
                        <div className="max-w-7xl mx-auto sm:px-6 lg:px-8">
                            <div className="bg-white overflow-hidden shadow-sm sm:rounded-lg">
                                <div className="p-6 bg-white border-b border-gray-200">
                                    <div className="mb-4">
                                        <label className="text-xl text-gray-600">Title <span className="text-red-500">*</span></label><br/>
                                        <input 
                                            type="text" 
                                            className="border-2 border-gray-300 p-2 w-full" 
                                            name="title" 
                                            id="title" 
                                            value={title} 
                                            onChange={(e) => setTitle(e.target.value)} 
                                            required
                                        />
                                    </div>
                                    <div className="mb-4">
                                        <label className="text-xl text-gray-600">Type</label><br/>
                                        <Select
                                            placeholder="Select Type"
                                            value={type} 
                                            onChange={(e) => setType(e.target.value)}
                                        >
                                            <option value='Blog'>Blog</option>
                                            <option value='Promotion'>Promotion</option>
                                        </Select>
                                    </div>
                                    <div className="mb-4">
                                        <label className="text-xl text-gray-600">Image</label><br/>
                                        <Input
                            type="file"
                            onChange={handleImageChange}
                        />
                    
                                    </div>
                                    <div className="mb-4">
                                        <label className="text-xl text-gray-600">Description</label><br/>
                                        <input 
                                            type="text" 
                                            className="border-2 border-gray-300 p-2 w-full" 
                                            name="description" 
                                            id="description" 
                                            placeholder="(Optional)" 
                                            value={description} 
                                            onChange={(e) => setDescription(e.target.value)}
                                        />
                                    </div>
                                    <div className="mb-8">
                                        <label className="text-xl text-gray-600">Content <span className="text-red-500">*</span></label><br/>
                                        <textarea 
                                            name="content" 
                                            className="border-2 border-gray-500" 
                                            value={content}
                                            onChange={(e) => setContent(e.target.value)}
                                        >
                                        </textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ModalBody>

                <ModalFooter>
                    <Button onClick={onSave}>Submit</Button>
                    <Button onClick={onClose}>Cancel</Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
};

CreateBlogModal.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired,
    fetchBlogs: PropTypes.func.isRequired,
};

export default CreateBlogModal;
