import { useState, useEffect } from "react";
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalCloseButton, ModalBody, ModalFooter, FormControl, FormLabel, Input, Select, Button } from "@chakra-ui/react";
import axios from "axios";
import PropTypes from "prop-types";

const UpdateTheaterModal = ({ theater, isOpen, onClose, fetchTheaters }) => {
    const [name, setName] = useState(theater.name);
    const [email, setEmail] = useState(theater.email);
    const [phone, setPhone] = useState(theater.phone);
    const [address, setAddress] = useState(theater.address);
    const [status, setStatus] = useState(theater.status);
    const [scrollBehavior] = useState('inside');

    useEffect(() => {
        // Reset form state when the modal opens
        if (theater) {
            setName(theater.name);
            setEmail(theater.email);
            setPhone(theater.phone);
            setAddress(theater.address);
            setStatus(theater.status);
        }
    }, [theater, isOpen]);

    const onUpdate = async () => {
        const token = localStorage.getItem("jwtToken");

        const data = {
            id: theater.id, // Use the theater ID
            name: theater.name,
            email: theater.email,
            address: theater.address,
            phone: theater.phone,
            gender: theater.gender, 
            birthdate: theater.birthdate, // Assuming birthdate is part of theater object
            password: theater.password, // Assuming password is part of theater object
            created_date: theater.created_date, // Assuming created_date is part of theater object
            status: status || null,
        };

        try {
            await axios.put(`http://localhost:8000/TheaterService/${theater.id}`, data, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                    Site: 'admin',
                },
            });
            onClose(); // Close modal on success
            fetchTheaters();
        } catch (error) {
            console.error("Error updating theater:", error.response ? error.response.data : error.message);
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
                        <Input
                            placeholder="Address"
                            value={address}
                            onChange={(e) => setAddress(e.target.value)}
                        />
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

UpdateTheaterModal.propTypes = {
    theater: PropTypes.object.isRequired,
    isOpen: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired,
    fetchTheaters: PropTypes.func.isRequired
};

export default UpdateTheaterModal;
