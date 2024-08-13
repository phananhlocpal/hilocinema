import { useState, useEffect } from "react";
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalCloseButton, ModalBody, ModalFooter, FormControl, FormLabel, Input, Textarea, Select, Button, Image } from "@chakra-ui/react";
import axios from "axios";
import PropTypes from "prop-types";
import { toBase64 } from "../../utils/toBase64";

const UpdateCustomerModal = ({ customer, isOpen, onClose, fetchCustomers }) => {
    const [name, setName] = useState(customer.name);
    const [email, setEmail] = useState(customer.email);
    const [phone, setPhone] = useState(customer.phone);
    const [address, setAddress] = useState(customer.address);
    const [profilePicture, setProfilePicture] = useState(null);
    const [profilePicturePreview, setProfilePicturePreview] = useState(customer.profilePicture);
    const [status, setStatus] = useState(customer.status);
    const [scrollBehavior] = useState('inside');

    useEffect(() => {
        // Reset form state when the modal opens
        if (customer) {
            setName(customer.name);
            setEmail(customer.email);
            setPhone(customer.phone);
            setAddress(customer.address);
            setProfilePicture(null);
            setProfilePicturePreview(customer.profilePicture);
            setStatus(customer.status);
        }
    }, [customer, isOpen]);

    const handleProfilePictureChange = (event) => {
        const file = event.target.files[0];
        setProfilePicture(file);
        const reader = new FileReader();
        reader.onloadend = () => {
            setProfilePicturePreview(reader.result);
        };
        if (file) {
            reader.readAsDataURL(file);
        }
    };

    const onUpdate = async () => {
        const token = localStorage.getItem("jwtToken");

        let profilePictureBase64 = '';
        if (profilePicture) {
            profilePictureBase64 = await toBase64(profilePicture);
        }

        const data = {
            name,
            email,
            phone,
            address,
            profilePicture: profilePictureBase64 || customer.profilePicture,
            status: status || null
        };

        try {
            await axios.put(`http://localhost:8000/CustomerService/${customer.id}`, data, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                    Site: 'admin',
                    Role: 'admin'
                },
            });
            onClose(); // Close modal on success
            fetchCustomers();
        } catch (error) {
            console.error("Error updating customer:", error.response ? error.response.data : error.message);
        }
    };

    return (
        <Modal isOpen={isOpen} onClose={onClose} size={"xl"} scrollBehavior={scrollBehavior}>
            <ModalOverlay />
            <ModalContent>
                <ModalHeader>Cập nhật khách hàng</ModalHeader>
                <ModalCloseButton />
                <ModalBody pb={6}>
                    <FormControl>
                        <FormLabel>Name</FormLabel>
                        <Input
                            placeholder="Name"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                        />
                    </FormControl>
                    <FormControl mt={4}>
                        <FormLabel>Email</FormLabel>
                        <Input
                            type='email'
                            placeholder="Email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </FormControl>
                    <FormControl mt={4}>
                        <FormLabel>Phone</FormLabel>
                        <Input
                            type='tel'
                            placeholder="Phone"
                            value={phone}
                            onChange={(e) => setPhone(e.target.value)}
                        />
                    </FormControl>
                    <FormControl mt={4}>
                        <FormLabel>Address</FormLabel>
                        <Textarea
                            placeholder="Address"
                            value={address}
                            onChange={(e) => setAddress(e.target.value)}
                        />
                    </FormControl>
                    <FormControl mt={4}>
                        <FormLabel>Profile Picture</FormLabel>
                        <Input
                            type="file"
                            onChange={handleProfilePictureChange}
                        />
                        {profilePicturePreview && (
                            <Image
                                src={profilePicturePreview}
                                alt="Profile Picture Preview"
                                mt={2}
                                boxSize="150px"
                                objectFit="cover"
                            />
                        )}
                    </FormControl>
                    <FormControl mt={4}>
                        <FormLabel>Status</FormLabel>
                        <Select
                            placeholder="Select Status"
                            value={status}
                            onChange={(e) => setStatus(e.target.value)}
                        >
                            <option value='Active'>Active</option>
                            <option value='Inactive'>Inactive</option>
                        </Select>
                    </FormControl>
                </ModalBody>
                <ModalFooter>
                    <Button colorScheme="blue" mr={3} onClick={onUpdate}>
                        Update
                    </Button>
                    <Button onClick={onClose}>Cancel</Button>
                </ModalFooter>
            </ModalContent>
        </Modal>
    );
};

UpdateCustomerModal.propTypes = {
    customer: PropTypes.object.isRequired,
    isOpen: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired,
    fetchCustomers: PropTypes.func.isRequired
};

export default UpdateCustomerModal;
